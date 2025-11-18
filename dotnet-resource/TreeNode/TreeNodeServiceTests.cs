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
    public void GetTree_ParentChildHierarchy_BuildsCorrectTree_randomizedFlatNodes()
    {
        // Arrange
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

        // randomize flatNodes
        Random rnd = new();
        flatNodes = flatNodes.OrderBy(x => rnd.Next()).ToList();

        // Act
        var result = _service.GetTree(flatNodes);

        // Assert
        Assert.HasCount(1, result);
        Assert.HasCount(6, result[0].Children);
        Assert.HasCount(3, result[0].Children.Where(t => t.Name.StartsWith("Leaf")).ToList());
        Assert.HasCount(3, result[0].Children.Where(t => t.Name.StartsWith("Branch")).ToList());
        var branchNodes = result[0].Children.Where(t => t.Name.StartsWith("Branch")).ToList();
        Assert.HasCount(2, branchNodes.Where(branchNode => branchNode.Children.Count == 3).ToList());
        Assert.HasCount(1, branchNodes.Where(branchNode => branchNode.Children.Count == 4).ToList());
        var branchNodeWith4Children = branchNodes.Where(branchNode => branchNode.Children.Count == 4).ToList()[0];
        Assert.HasCount(3,
            branchNodeWith4Children.Children.Where(leafNode => leafNode.Name.StartsWith("Leaf")).ToList());
        Assert.HasCount(1,
            branchNodeWith4Children.Children.Where(leafNode => leafNode.Name.StartsWith("Branch")).ToList());
        var leafNodeWithBranchChild =
            branchNodeWith4Children.Children.Where(leafNode => leafNode.Name.StartsWith("Branch")).ToList()[0];
        Assert.HasCount(4, leafNodeWithBranchChild.Children);
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