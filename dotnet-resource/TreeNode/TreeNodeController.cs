using Microsoft.AspNetCore.Mvc;

namespace dotnet_resource.TreeNode;

[ApiController]
[Route("treenode")]
public class TreeNodeController(TreeNodeDbContext context) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(context.TreeNodes);
    }

    [HttpPost]
    public IActionResult Post([FromBody] TreeNode node)
    {
        context.TreeNodes.Add(node);
        context.SaveChanges();
        return Ok(node);
    }
}