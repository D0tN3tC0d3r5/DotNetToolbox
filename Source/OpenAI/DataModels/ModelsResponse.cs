namespace DotNetToolbox.OpenAI.DataModels;

internal record ModelsResponse {
    public required OpenAiModel[] Data { get; init; }
}
