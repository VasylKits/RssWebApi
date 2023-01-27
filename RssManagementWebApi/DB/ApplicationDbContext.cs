using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RssManagementWebApi.Models;

namespace RssManagementWebApi.DB;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<Feed> Feeds { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions options)
    : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Feed>(entity =>
        {
            entity.Property(f => f.Id).ValueGeneratedOnAdd();
            entity.Property(f => f.Unread).HasDefaultValue(true);
        });
    }
}