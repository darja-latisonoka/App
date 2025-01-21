using Hermes.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Hermes.Data.Configurations;

public class PrivateChatConfiguration : IEntityTypeConfiguration<PrivateChat>
{
    public void Configure(EntityTypeBuilder<PrivateChat> builder)
    {
        builder.HasKey(u => u.Id);

        builder
            .HasOne(pc => pc.User1)
            .WithMany()
            .HasForeignKey(pc => pc.User1Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(pc => pc.User2)
            .WithMany()
            .HasForeignKey(pc => pc.User2Id)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasMany(pc => pc.Messages)
            .WithOne(m => m.PrivateChat)
            .HasForeignKey(m => m.PrivateChatId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}