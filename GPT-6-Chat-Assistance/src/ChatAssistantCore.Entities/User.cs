using System;
using System.Collections.Generic;

namespace ChatAssistantCore.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();
}
