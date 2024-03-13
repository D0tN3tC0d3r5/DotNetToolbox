namespace DotNetToolbox.AI.OpenAI.Model;

internal record OpenAIModelsResponse {
    public required OpenAIModel[] Data { get; init; }
}
