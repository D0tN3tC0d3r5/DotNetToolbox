namespace DotNetToolbox.OpenAI.Models.DataModels;

internal record ModelsResponse {
    public required OpenAiModel[] Data { get; init; }
}
