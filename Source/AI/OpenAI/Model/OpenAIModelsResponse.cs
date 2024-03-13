namespace DotNetToolbox.AI.OpenAI.Models;

internal record OpenAIModelsResponse {
    public required OpenAIModel[] Data { get; init; }
}
