using System;
using System.Collections.Generic;

namespace ChatAssistantCore.Entities;

public class Conversation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = "New Chat";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation property
    public List<Message> Messages { get; set; } = new();
}
