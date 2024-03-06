namespace DotNetToolbox.AI.OpenAI.DataModels;

internal record DeleteResponse {
    public required string Id { get; init; }
    public bool Deleted { get; init; }
}
