namespace dotnet_resource.TreeNode;

public class TreeNode
{
    public long Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    
    public List<TreeNode> Children { get; set; } = [];
}