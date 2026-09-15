using System;
using System.Collections.Generic;

namespace ChatAssistantCore.DTO.Chat;

public class ConversationDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<MessageDto> Messages { get; set; } = new();
}

public class MessageDto
{
    public Guid Id { get; set; }
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? ModelUsed { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ChatRequestDto
{
    public string Message { get; set; } = string.Empty;
}

public class ChatResponseDto
{
    public MessageDto AssistantMessage { get; set; } = new();
}
