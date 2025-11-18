using Microsoft.AspNetCore.Mvc;

namespace dotnet_resource.TreeNode;

[ApiController]
[Route("api/treenode")]
public class TreeNodeController : ControllerBase
{
    private readonly ITreeNodeService service;
    public TreeNodeController(ITreeNodeService service) => this.service = service;
    
    [HttpGet]
    public IActionResult Get()
    {
        var flatNodes = new List<FlatNode.FlatNode>
        {
            new() { Id = 1, Name = "Root", ParentId = null },
            new() { Id = 2, Name = "Leaf_1", ParentId = 1 },
            new() { Id = 3, Name = "Leaf_2", ParentId = 1 },
            new() { Id = 4, Name = "Leaf_3", ParentId = 1 },
            new() { Id = 5, Name = "Branch_1", ParentId = 1 },
            new() { Id = 6, Name = "Leaf_4", ParentId = 5 },
            new() { Id = 7, Name = "Leaf_5", ParentId = 5 },
            new() { Id = 8, Name = "Leaf_6", ParentId = 5 },
            new() { Id = 9, Name = "Branch_2", ParentId = 1 },
            new() { Id = 10, Name = "Leaf_7", ParentId = 9 },
            new() { Id = 11, Name = "Leaf_8", ParentId = 9 },
            new() { Id = 12, Name = "Leaf_9", ParentId = 9 },
            new() { Id = 13, Name = "Branch_3", ParentId = 1 },
            new() { Id = 14, Name = "Leaf_10", ParentId = 13 },
            new() { Id = 15, Name = "Leaf_11", ParentId = 13 },
            new() { Id = 16, Name = "Leaf_12", ParentId = 13 },
            new() { Id = 17, Name = "Branch_3_Branch_4", ParentId = 13 },
            new() { Id = 18, Name = "Leaf_13", ParentId = 17 },
            new() { Id = 19, Name = "Leaf_14", ParentId = 17 },
            new() { Id = 20, Name = "Leaf_15", ParentId = 17 },
            new() { Id = 21, Name = "Branch_3_Branch_4_Branch_5", ParentId = 17 },
            new() { Id = 22, Name = "Leaf_16", ParentId = 21 },
            new() { Id = 23, Name = "Leaf_17", ParentId = 21 },
            new() { Id = 24, Name = "Leaf_18", ParentId = 21 }
        };

        var data = service.GetTree(flatNodes);
        return Ok(data);
    }
}