using Microsoft.EntityFrameworkCore;

namespace dotnet_resource.FlatNode;

public class FlatNodeDbContext(DbContextOptions<FlatNodeDbContext> options) : DbContext(options)
{
    public DbSet<FlatNode> TreeNodes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FlatNode>()
            .HasAlternateKey(t => t.Name);
    }
}