namespace DotNetToolbox.OpenAI.Chats.DataModels;

internal record CompletionResponse {
    public required string Id { get; init; }
    public string? Model { get; init; }
    public Choice[] Choices { get; init; } = [];
    public int Created { get; init; }
    public string? SystemFingerprint { get; init; }
    public Usage? Usage { get; init; }
}
