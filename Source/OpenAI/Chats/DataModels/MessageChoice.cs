namespace DotNetToolbox.OpenAI.Chats.DataModels;

internal record MessageChoice : Choice {
    public required Message Message { get; init; }
}