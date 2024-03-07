namespace DotNetToolbox.AI.OpenAI.DataModels;

internal record ModelsResponse {
    public required Model[] Data { get; init; }
}
