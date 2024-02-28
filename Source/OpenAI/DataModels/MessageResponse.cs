namespace DotNetToolbox.OpenAI.DataModels;

internal record MessageResponse : CompletionResponse {
    public MessageChoice[] Choices { get; init; } = [];
}