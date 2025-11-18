namespace dotnet_resource.TreeNode;

public class TreeNode
{
    public long Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public long? ParentId { get; set; }
}