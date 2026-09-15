using System;
using System.Collections.Generic;

namespace ChatAssistantCore.Entities;

public class Conversation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = "New Chat";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation property
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public ICollection<Message> Messages { get; set; } = new List<Message>();
}
