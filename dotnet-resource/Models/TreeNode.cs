using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace dotnet_resource.Models;

public class TreeNode
{
    public long Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public long? ParentId { get; set; }

    public TreeNode? Parent { get; set; }

    public ICollection<TreeNode> Children { get; set; } = new List<TreeNode>();
}

public class TreeNodeDbContext : DbContext
{
    public TreeNodeDbContext(DbContextOptions<TreeNodeDbContext> options) : base(options)
    {
    }

    public DbSet<TreeNode> TreeNodes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TreeNode>()
            .HasOne(t => t.Parent)
            .WithMany(t => t.Children)
            .HasForeignKey(t => t.ParentId)
            .IsRequired(false);
    }
}