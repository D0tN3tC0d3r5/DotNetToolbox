namespace DotNetToolbox.AI.Jobs;

public class Asset(string id) {
    public string Id { get; } = id;
    public required string Name { get; init; }
    public required AssetType Type { get; init; }
    public required string Path { get; init; }
}
