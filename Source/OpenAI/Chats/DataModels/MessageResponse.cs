namespace DotNetToolbox.OpenAI.Chats.DataModels;

internal record MessageResponse : CompletionResponse {
    public MessageChoice[] Choices { get; init; } = [];
}