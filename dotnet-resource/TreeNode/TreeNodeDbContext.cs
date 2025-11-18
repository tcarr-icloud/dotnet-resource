using Microsoft.EntityFrameworkCore;

namespace dotnet_resource.TreeNode;

public class TreeNodeDbContext(DbContextOptions<TreeNodeDbContext> options) : DbContext(options)
{
    public DbSet<TreeNode> TreeNodes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TreeNode>()
            .HasAlternateKey(t => t.Name);
    }
}