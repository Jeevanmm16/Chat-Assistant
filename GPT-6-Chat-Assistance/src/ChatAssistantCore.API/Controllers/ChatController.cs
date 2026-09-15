using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatAssistantCore.API.Constants;
using ChatAssistantCore.DTO.Chat;
using ChatAssistantCore.Service.Chat;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ChatAssistantCore.API.Controllers;

[ApiController]
[Authorize]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;

    public ChatController(IChatService chatService)
    {
        _chatService = chatService;
    }

    [HttpGet]
    [Route(APIUrlConstants.GetConversations)]
    [SwaggerOperation(Summary = "Get All Conversations", Description = "Retrieves a list of all chat conversations.")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<ConversationDto>))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> GetConversations()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out var userId)) return Unauthorized();

        var result = await _chatService.GetConversationsAsync(userId);
        return Ok(result);
    }

    [HttpGet]
    [Route(APIUrlConstants.GetConversation)]
    [SwaggerOperation(Summary = "Get Conversation By Id", Description = "Retrieves a conversation and its messages.")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ConversationDto))]
    [SwaggerResponse(StatusCodes.Status404NotFound, Type = typeof(string))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> GetConversation(Guid id)
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out var userId)) return Unauthorized();

        var result = await _chatService.GetConversationAsync(id, userId);
        
        if (result == null)
            return NotFound("Conversation not found");
            
        return Ok(result);
    }

    [HttpPost]
    [Route(APIUrlConstants.CreateConversation)]
    [SwaggerOperation(Summary = "Create Conversation", Description = "Creates a new empty conversation.")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ConversationDto))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> CreateConversation()
    {
        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out var userId)) return Unauthorized();

        var result = await _chatService.CreateConversationAsync(userId);
        return Ok(result);
    }

    [HttpPost]
    [Route(APIUrlConstants.SendMessage)]
    [SwaggerOperation(Summary = "Send Message", Description = "Sends a message to the AI for a specific conversation.")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(ChatResponseDto))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> SendMessage(Guid id, [FromBody] ChatRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Message))
            return BadRequest("Message cannot be empty");

        var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdStr, out var userId)) return Unauthorized();

        var result = await _chatService.SendMessageAsync(id, userId, request);
        return Ok(result);
    }
}
