using Hermes.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hermes.Data.Configurations;

public class GroupChatMembershipConfiguration : IEntityTypeConfiguration<GroupChatMembership>
{
    public void Configure(EntityTypeBuilder<GroupChatMembership> builder)
    {
        builder.HasKey(gcm => gcm.Id);

        builder.HasOne(gcm => gcm.GroupChat)
            .WithMany(gc => gc.Memberships)
            .HasForeignKey(gcm => gcm.GroupChatId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(gcm => gcm.User)
            .WithMany(u => u.GroupChatMemberships)
            .HasForeignKey(gcm => gcm.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(gcm => gcm.IsAdmin);
    }
}