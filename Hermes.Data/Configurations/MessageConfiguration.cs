using Hermes.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hermes.Data.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(m => m.Id);

        builder
            .HasOne(m => m.Sender)
            .WithMany()
            .HasForeignKey(m => m.SenderId);

        builder
            .HasOne(m => m.PrivateChat)
            .WithMany(pc => pc.Messages)
            .HasForeignKey(m => m.PrivateChatId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(m => m.Content);

        builder.Property(m => m.SentAt);

        builder.HasOne(m => m.GroupChat)
            .WithMany(gc => gc.Messages)
            .HasForeignKey(m => m.GroupChatId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}