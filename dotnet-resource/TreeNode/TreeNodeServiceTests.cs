namespace dotnet_resource.TreeNode;

[TestClass]
public class TreeNodeServiceTests
{
    private readonly TreeNodeService _service = new();

    [TestMethod]
    public void GetTree_SingleRootNode_ReturnsRootWithNoChildren()
    {
        // Arrange
        var flatNodes = new List<FlatNode.FlatNode>
        {
            new() { Id = 1, Name = "Root", ParentId = null }
        };

        // Act
        var result = _service.GetTree(flatNodes);

        // Assert
        Assert.HasCount(1, result);
        Assert.AreEqual(1, result[0].Id);
        Assert.AreEqual("Root", result[0].Name);
        Assert.IsEmpty(result[0].Children);
    }

    [TestMethod]
    public void GetTree_ParentChildHierarchy_BuildsCorrectTree()
    {
        // Arrange
        var flatNodes = new List<FlatNode.FlatNode>
        {
            new() { Id = 1, Name = "Root", ParentId = null },
            new() { Id = 2, Name = "Child1", ParentId = 1 },
            new() { Id = 3, Name = "Child2", ParentId = 1 }
        };

        // Act
        var result = _service.GetTree(flatNodes);

        // Assert
        Assert.HasCount(1, result);
        Assert.AreEqual(1, result[0].Id);
        Assert.HasCount(2, result[0].Children);
        Assert.AreEqual(2, result[0].Children[0].Id);
        Assert.AreEqual("Child1", result[0].Children[0].Name);
        Assert.AreEqual(3, result[0].Children[1].Id);
        Assert.AreEqual("Child2", result[0].Children[1].Name);
    }

    [TestMethod]
    public void GetTree_ParentChildHierarchy_BuildsCorrectTree_FlatnodesReversed()
    {
        // Arrange
        var flatNodes = new List<FlatNode.FlatNode>
        {
            new() { Id = 3, Name = "Child2", ParentId = 1 },
            new() { Id = 2, Name = "Child1", ParentId = 1 },
            new() { Id = 1, Name = "Root", ParentId = null }
        };

        // Act
        var result = _service.GetTree(flatNodes);

        // Assert
        Assert.HasCount(1, result);
        Assert.AreEqual(1, result[0].Id);
        Assert.HasCount(2, result[0].Children);
        Assert.AreEqual(3, result[0].Children[0].Id);
        Assert.AreEqual("Child2", result[0].Children[0].Name);
        Assert.AreEqual(2, result[0].Children[1].Id);
        Assert.AreEqual("Child1", result[0].Children[1].Name);
    }

    [TestMethod]
    public void GetTree_MultipleRoots_ReturnsAllRoots()
    {
        // Arrange
        var flatNodes = new List<FlatNode.FlatNode>
        {
            new() { Id = 1, Name = "Root1", ParentId = null },
            new() { Id = 2, Name = "Root2", ParentId = null },
            new() { Id = 3, Name = "Child", ParentId = 1 }
        };

        // Act
        var result = _service.GetTree(flatNodes);

        // Assert
        Assert.HasCount(2, result);
        Assert.AreEqual(1, result[0].Id);
        Assert.HasCount(1, result[0].Children);
        Assert.AreEqual(2, result[1].Id);
        Assert.IsEmpty(result[1].Children);
    }

    [TestMethod]
    public void GetTree_OrphanNode_TreatsAsRoot()
    {
        // Arrange
        var flatNodes = new List<FlatNode.FlatNode>
        {
            new() { Id = 1, Name = "Root", ParentId = null },
            new() { Id = 2, Name = "Orphan", ParentId = 999 }
        };

        // Act
        var result = _service.GetTree(flatNodes);

        // Assert
        Assert.HasCount(2, result);
        Assert.IsTrue(result.Any(n => n.Id == 1 && n.Name == "Root"));
        Assert.IsTrue(result.Any(n => n.Id == 2 && n.Name == "Orphan"));
    }

    [TestMethod]
    public void GetTree_EmptyList_ReturnsEmptyList()
    {
        // Arrange
        var flatNodes = new List<FlatNode.FlatNode>();

        // Act
        var result = _service.GetTree(flatNodes);

        // Assert
        Assert.IsEmpty(result);
    }

    [TestMethod]
    public void GetTree_MultiLevelHierarchy_BuildsCompleteTree()
    {
        // Arrange
        var flatNodes = new List<FlatNode.FlatNode>
        {
            new() { Id = 1, Name = "Root", ParentId = null },
            new() { Id = 2, Name = "Child", ParentId = 1 },
            new() { Id = 3, Name = "Grandchild", ParentId = 2 },
            new() { Id = 4, Name = "GreatGrandchild", ParentId = 3 }
        };

        // Act
        var result = _service.GetTree(flatNodes);

        // Assert
        Assert.HasCount(1, result);
        Assert.AreEqual(1, result[0].Id);
        Assert.HasCount(1, result[0].Children);
        Assert.AreEqual(2, result[0].Children[0].Id);
        Assert.HasCount(1, result[0].Children[0].Children);
        Assert.AreEqual(3, result[0].Children[0].Children[0].Id);
        Assert.HasCount(1, result[0].Children[0].Children[0].Children);
        Assert.AreEqual(4, result[0].Children[0].Children[0].Children[0].Id);
    }

    [TestMethod]
    public void GetTree_UsesDescriptionWhenAvailable_OtherwiseName()
    {
        // Arrange
        var flatNodes = new List<FlatNode.FlatNode>
        {
            new() { Id = 1, Name = "Name1", Description = "Description1", ParentId = null },
            new() { Id = 2, Name = "Name2", Description = null, ParentId = null }
        };

        // Act
        var result = _service.GetTree(flatNodes);

        // Assert
        Assert.AreEqual("Description1", result[0].Name);
        Assert.AreEqual("Name2", result[1].Name);
    }
}