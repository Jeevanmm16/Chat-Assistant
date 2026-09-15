using System;

namespace ChatAssistantCore.Entities;

public class Message
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ConversationId { get; set; }
    public string Role { get; set; } = "user"; // "user", "assistant"
    public string Content { get; set; } = string.Empty;
    public string? ModelUsed { get; set; } // e.g. "GPT-6-Astra", "GPT-5-mini"
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public Conversation? Conversation { get; set; }
}
