namespace DotNetToolbox.AI.OpenAI.Model;

internal record ModelsResponse {
    public required Model[] Data { get; init; }
}
