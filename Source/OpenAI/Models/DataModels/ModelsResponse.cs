namespace DotNetToolbox.OpenAI.Models.DataModels;

internal record ModelsResponse {
    public required Model[] Data { get; init; }
}
