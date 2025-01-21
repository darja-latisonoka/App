using Hermes.Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Hermes.Data;

public class HermesDbContext(IConfiguration configuration) : IdentityDbContext<User>
{
    private DbSet<PrivateChat> PrivateChats { get; set; }
    private DbSet<Message> Messages { get; set; }
    private DbSet<GroupChat> GroupChats { get; set; }
    private DbSet<GroupChatMembership> GroupChatMemberships { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var azureDbConnectionString = Environment.GetEnvironmentVariable("SQLCONNSTR_AzureDB");

        if (string.IsNullOrEmpty(azureDbConnectionString))
        {
            throw new InvalidOperationException("The SQLCONNSTR_AzureDB environment variable is not set.");
        }

        optionsBuilder.UseSqlServer(azureDbConnectionString);
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(HermesDbContext).Assembly);
    }
}