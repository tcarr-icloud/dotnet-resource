using Microsoft.AspNetCore.Mvc;

namespace dotnet_resource.TreeNode;

[ApiController]
[Route("api/treenode")]
public class TreeNodeController(TreeNodeDbContext context) : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(context.TreeNodes);
    }

    [HttpGet("{id:long}")]
    public IActionResult Get(long id)
    {
        var node = context.TreeNodes.Find(id);
        if (node == null) return NotFound();
        return Ok(node);
    }

    [HttpPost]
    public IActionResult Post([FromBody] TreeNode node)
    {
        context.TreeNodes.Add(node);
        context.SaveChanges();
        return Ok(node);
    }

    [HttpPut("{id:long}")]
    public IActionResult Put(long id, [FromBody] TreeNode node)
    {
        var existingNode = context.TreeNodes.Find(id);
        if (existingNode == null) return NotFound();

        existingNode.Name = node.Name;
        existingNode.Description = node.Description;
        context.SaveChanges();
        return Ok(existingNode);
    }

    [HttpDelete("{id:long}")]
    public IActionResult Delete(long id)
    {
        var node = context.TreeNodes.Find(id);
        if (node == null) return NotFound();

        context.TreeNodes.Remove(node);
        context.SaveChanges();
        return Ok();
    }
}