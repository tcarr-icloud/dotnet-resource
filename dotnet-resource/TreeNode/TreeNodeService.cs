namespace dotnet_resource.TreeNode;

public interface ITreeNodeService
{
    List<TreeNode> GetTree(List<FlatNode.FlatNode> nodes);
}

public class TreeNodeService : ITreeNodeService
{
    public List<TreeNode> GetTree(List<FlatNode.FlatNode> flatNodes)
    {
        var map = flatNodes.ToDictionary(
            n => n.Id,
            n => new TreeNode { Id = n.Id, Name = n.Description ?? n.Name });

        var roots = new List<TreeNode>();

        foreach (var flat in flatNodes)
        {
            var current = map[flat.Id];

            if (flat.ParentId is null)
                // Root node
                roots.Add(current);
            else if (map.TryGetValue(flat.ParentId.Value, out var parent))
                parent.Children.Add(current);
            else
                // Orphan: parent not found, treat as root (optional behavior)
                roots.Add(current);
        }

        return roots;
    }
}