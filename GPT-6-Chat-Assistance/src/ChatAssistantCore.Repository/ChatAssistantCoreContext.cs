using ChatAssistantCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatAssistantCore.Repository;

public class ChatAssistantCoreContext : DbContext
{
    public ChatAssistantCoreContext(DbContextOptions<ChatAssistantCoreContext> options)
        : base(options)
    {
    }

    public DbSet<Conversation> Conversations => Set<Conversation>();
    public DbSet<Message> Messages => Set<Message>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(255);
            entity.HasMany(e => e.Messages)
                  .WithOne(e => e.Conversation)
                  .HasForeignKey(e => e.ConversationId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Role).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Content).IsRequired();
            entity.Property(e => e.ModelUsed).HasMaxLength(100);
        });
    }
}
