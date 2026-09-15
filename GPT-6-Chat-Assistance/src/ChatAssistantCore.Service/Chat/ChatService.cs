using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ChatAssistantCore.DTO.Chat;
using ChatAssistantCore.Entities;
using ChatAssistantCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ChatAssistantCore.Service.Chat;

public class ChatService : IChatService
{
    private readonly ChatAssistantCoreContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ChatService> _logger;
    private readonly HttpClient _httpClient;

    public ChatService(ChatAssistantCoreContext context, IConfiguration configuration, ILogger<ChatService> logger, HttpClient httpClient)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<List<ConversationDto>> GetConversationsAsync()
    {
        var conversations = await _context.Conversations
            .AsNoTracking()
            .OrderByDescending(c => c.CreatedAt)
            .Select(c => new ConversationDto
            {
                Id = c.Id,
                Title = c.Title,
                CreatedAt = c.CreatedAt
            })
            .ToListAsync();

        return conversations;
    }

    public async Task<ConversationDto?> GetConversationAsync(Guid id)
    {
        var conversation = await _context.Conversations
            .AsNoTracking()
            .Include(c => c.Messages.OrderBy(m => m.CreatedAt))
            .FirstOrDefaultAsync(c => c.Id == id);

        if (conversation == null) return null;

        return new ConversationDto
        {
            Id = conversation.Id,
            Title = conversation.Title,
            CreatedAt = conversation.CreatedAt,
            Messages = conversation.Messages.Select(m => new MessageDto
            {
                Id = m.Id,
                Role = m.Role,
                Content = m.Content,
                ModelUsed = m.ModelUsed,
                CreatedAt = m.CreatedAt
            }).ToList()
        };
    }

    public async Task<ConversationDto> CreateConversationAsync()
    {
        var conversation = new Conversation
        {
            Id = Guid.NewGuid(),
            Title = "New Chat",
            CreatedAt = DateTime.UtcNow
        };

        _context.Conversations.Add(conversation);
        await _context.SaveChangesAsync();

        return new ConversationDto
        {
            Id = conversation.Id,
            Title = conversation.Title,
            CreatedAt = conversation.CreatedAt
        };
    }

    public async Task<ChatResponseDto> SendMessageAsync(Guid conversationId, ChatRequestDto request)
    {
        var conversation = await _context.Conversations
            .Include(c => c.Messages.OrderBy(m => m.CreatedAt))
            .FirstOrDefaultAsync(c => c.Id == conversationId);

        if (conversation == null) throw new Exception("Conversation not found");

        // 1. Add user message
        var userMessage = new Message
        {
            Id = Guid.NewGuid(),
            ConversationId = conversationId,
            Role = "user",
            Content = request.Message,
            CreatedAt = DateTime.UtcNow
        };
        _context.Messages.Add(userMessage);
        
        if (conversation.Title == "New Chat" && conversation.Messages.Count == 0)
        {
            // Simple auto-titling for the first message
            conversation.Title = string.Join(" ", request.Message.Split(' ').Take(5)) + "...";
        }
        
        await _context.SaveChangesAsync();

        // Prepare context for Azure OpenAI
        var allMessages = conversation.Messages.OrderBy(m => m.CreatedAt).Select(m => new { role = m.Role, content = m.Content }).ToList();
        allMessages.Add(new { role = "user", content = request.Message });

        var apiKey = _configuration["AzureOpenAI:ApiKey"];
        var gpt6Url = _configuration["AzureOpenAI:GPT6Url"];
        var gpt5Url = _configuration["AzureOpenAI:GPT5Url"];

        if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(gpt6Url) || string.IsNullOrEmpty(gpt5Url))
        {
            throw new Exception("Azure OpenAI configuration is missing.");
        }

        using var requestMessage = new HttpRequestMessage();
        requestMessage.Headers.Add("api-key", apiKey);
        requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        string? assistantContent = null;
        string modelUsed = "";

        try
        {
            _logger.LogInformation("Attempting GPT-6-Astra for conversation {ConversationId}", conversationId);
            
            var payload = new
            {
                model = "gpt-6-Astra",
                input = allMessages
            };
            
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var req = new HttpRequestMessage(HttpMethod.Post, gpt6Url) { Content = content };
            req.Headers.Add("api-key", apiKey);

            using var cts = new System.Threading.CancellationTokenSource(TimeSpan.FromSeconds(10));
            var response = await _httpClient.SendAsync(req, cts.Token);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                assistantContent = ExtractMessageText(responseString);
                modelUsed = "GPT-6-Astra";
                
                if (string.IsNullOrEmpty(assistantContent)) throw new Exception("Could not parse GPT-6 response.");
            }
            else
            {
                _logger.LogWarning("GPT-6 failed with status {StatusCode}", response.StatusCode);
                throw new Exception("GPT-6 request failed.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "GPT-6 failed, falling back to GPT-5-mini for conversation {ConversationId}", conversationId);

            var payload = new
            {
                model = "gpt-5-mini",
                input = allMessages
            };
            
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var req = new HttpRequestMessage(HttpMethod.Post, gpt5Url) { Content = content };
            req.Headers.Add("api-key", apiKey);

            var response = await _httpClient.SendAsync(req);
            
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                assistantContent = ExtractMessageText(responseString);
                modelUsed = "GPT-5-mini";
                
                if (string.IsNullOrEmpty(assistantContent)) throw new Exception("Could not parse GPT-5 response.");
            }
            else
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                _logger.LogError("GPT-5 fallback also failed: {Error}", errorBody);
                throw new Exception("Both AI models failed to respond.");
            }
        }

        var assistantMessage = new Message
        {
            Id = Guid.NewGuid(),
            ConversationId = conversationId,
            Role = "assistant",
            Content = assistantContent,
            ModelUsed = modelUsed,
            CreatedAt = DateTime.UtcNow
        };
        
        _context.Messages.Add(assistantMessage);
        await _context.SaveChangesAsync();

        return new ChatResponseDto
        {
            AssistantMessage = new MessageDto
            {
                Id = assistantMessage.Id,
                Role = assistantMessage.Role,
                Content = assistantMessage.Content,
                ModelUsed = assistantMessage.ModelUsed,
                CreatedAt = assistantMessage.CreatedAt
            }
        };
    }

    private string? ExtractMessageText(string jsonResponse)
    {
        try
        {
            using var doc = JsonDocument.Parse(jsonResponse);
            var root = doc.RootElement;
            if (root.TryGetProperty("output", out var outputElement) && outputElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in outputElement.EnumerateArray())
                {
                    if (item.TryGetProperty("type", out var typeElement) && typeElement.GetString() == "message")
                    {
                        if (item.TryGetProperty("content", out var contentElement) && contentElement.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var c in contentElement.EnumerateArray())
                            {
                                if (c.TryGetProperty("type", out var cType) && cType.GetString() == "output_text")
                                {
                                    return c.GetProperty("text").GetString();
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error parsing JSON response");
        }
        return null;
    }
}
