using Microsoft.EntityFrameworkCore;
using RssManagementWebApi.Models;

namespace RssManagementWebApi.DB;

public class ApplicationDbContext : DbContext
{
    public DbSet<Feed> Feeds { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source = feedsapp.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Feed>().Property(f => f.Id)
        .ValueGeneratedOnAdd();
        modelBuilder.Entity<Feed>().Property(f => f.Unread).HasDefaultValue(true);
    }
}