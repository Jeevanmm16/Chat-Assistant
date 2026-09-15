using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatAssistantCore.DTO.Chat;

namespace ChatAssistantCore.Service.Chat;

public interface IChatService
{
    Task<List<ConversationDto>> GetConversationsAsync();
    Task<ConversationDto?> GetConversationAsync(Guid id);
    Task<ConversationDto> CreateConversationAsync();
    Task<ChatResponseDto> SendMessageAsync(Guid conversationId, ChatRequestDto request);
}
