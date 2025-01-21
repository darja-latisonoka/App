using Hermes.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hermes.Data.Configurations;

public class GroupChatConfiguration : IEntityTypeConfiguration<GroupChat>
{
    public void Configure(EntityTypeBuilder<GroupChat> builder)
    {
        builder.HasKey(gc => gc.Id);

        builder.HasMany(gc => gc.Memberships)
            .WithOne(gcm => gcm.GroupChat)
            .HasForeignKey(gcm => gcm.GroupChatId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(gc => gc.Name);

        builder.HasMany(gc => gc.Messages)
            .WithOne(m => m.GroupChat)
            .HasForeignKey(m => m.GroupChatId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}