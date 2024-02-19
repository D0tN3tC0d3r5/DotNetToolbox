namespace DotNetToolbox.OpenAI.Chats.DataModels;

internal record StreamResponse : CompletionResponse {
    public DeltaChoice[] Choices { get; init; } = [];
}