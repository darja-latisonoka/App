using Hermes.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hermes.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.IsOnline);

        builder.HasMany(u => u.GroupChatMemberships)
            .WithOne(gcm => gcm.User)
            .HasForeignKey(gcm => gcm.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}