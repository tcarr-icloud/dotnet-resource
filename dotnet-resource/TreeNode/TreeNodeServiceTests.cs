using FluentAssertions;
using Xunit;

namespace dotnet_resource.TreeNode;

public class TreeNodeServiceTests
{
    private readonly TreeNodeService _service = new();

    [Fact]
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
        result.Should().HaveCount(1);
        result[0].Id.Should().Be(1);
        result[0].Name.Should().Be("Root");
        result[0].Children.Should().BeEmpty();
    }

    [Fact]
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
        result.Should().HaveCount(1);
        result[0].Id.Should().Be(1);
        result[0].Children.Should().HaveCount(2);
        result[0].Children[0].Id.Should().Be(2);
        result[0].Children[0].Name.Should().Be("Child1");
        result[0].Children[1].Id.Should().Be(3);
        result[0].Children[1].Name.Should().Be("Child2");
    }

    [Fact]
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
        result.Should().HaveCount(2);
        result[0].Id.Should().Be(1);
        result[0].Children.Should().HaveCount(1);
        result[1].Id.Should().Be(2);
        result[1].Children.Should().BeEmpty();
    }

    [Fact]
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
        result.Should().HaveCount(2);
        result.Should().Contain(n => n.Id == 1 && n.Name == "Root");
        result.Should().Contain(n => n.Id == 2 && n.Name == "Orphan");
    }

    [Fact]
    public void GetTree_EmptyList_ReturnsEmptyList()
    {
        // Arrange
        var flatNodes = new List<FlatNode.FlatNode>();

        // Act
        var result = _service.GetTree(flatNodes);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
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
        result.Should().HaveCount(1);
        result[0].Id.Should().Be(1);
        result[0].Children.Should().HaveCount(1);
        result[0].Children[0].Id.Should().Be(2);
        result[0].Children[0].Children.Should().HaveCount(1);
        result[0].Children[0].Children[0].Id.Should().Be(3);
        result[0].Children[0].Children[0].Children.Should().HaveCount(1);
        result[0].Children[0].Children[0].Children[0].Id.Should().Be(4);
    }

    [Fact]
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
        result[0].Name.Should().Be("Description1");
        result[1].Name.Should().Be("Name2");
    }
}