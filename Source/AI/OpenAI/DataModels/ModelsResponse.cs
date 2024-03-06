namespace DotNetToolbox.AI.OpenAI.DataModels;

internal record ModelsResponse {
    public required OpenAiModel[] Data { get; init; }
}
