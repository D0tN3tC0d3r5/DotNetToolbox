namespace DotNetToolbox.OpenAI.Models.DataModels;

internal record DeleteResponse {
    public required string Id { get; init; }
    public bool Deleted { get; init; }
}
