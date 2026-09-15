using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChatAssistantCore.DTO.Chat;

namespace ChatAssistantCore.Service.Chat;

public interface IChatService
{
    Task<List<ConversationDto>> GetConversationsAsync(Guid userId);
    Task<ConversationDto?> GetConversationAsync(Guid id, Guid userId);
    Task<ConversationDto> CreateConversationAsync(Guid userId);
    Task<ChatResponseDto> SendMessageAsync(Guid conversationId, Guid userId, ChatRequestDto request);
}
