namespace DotNetToolbox.AI.OpenAI.DataModels;

internal record DeltaResponse : CompletionResponse {
    public DeltaChoice[] Choices { get; init; } = [];
}
