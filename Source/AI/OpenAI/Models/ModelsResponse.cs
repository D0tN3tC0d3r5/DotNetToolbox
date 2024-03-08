namespace DotNetToolbox.AI.OpenAI.Models;

internal record ModelsResponse {
    public required Model[] Data { get; init; }
}
