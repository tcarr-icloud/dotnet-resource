using System.ComponentModel.DataAnnotations;

namespace dotnet_resource.FlatNode;

public class FlatNode
{
    public long Id { get; init; }

    [StringLength(255)] public required string Name { get; set; }

    [StringLength(512)] public string? Description { get; set; }

    public long? ParentId { get; init; }
}
