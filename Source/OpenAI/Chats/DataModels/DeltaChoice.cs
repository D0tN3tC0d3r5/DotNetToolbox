namespace DotNetToolbox.OpenAI.Chats.DataModels;

internal record DeltaChoice : Choice {
    public required Message Delta { get; init; }
}