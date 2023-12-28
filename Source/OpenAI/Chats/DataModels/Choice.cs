namespace DotNetToolbox.OpenAI.Chats.DataModels;

internal record Choice {
    public int Index { get; init; }
    public required string FinishReason { get; init; }
    public required Message? Message { get; init; }
    public required Message? Delta { get; init; }
}
