namespace DotNetToolbox.OpenAI.DataModels;

internal record StreamResponse : CompletionResponse {
    public DeltaChoice[] Choices { get; init; } = [];
}