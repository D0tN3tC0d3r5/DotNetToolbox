namespace DotNetToolbox.OpenAI.Chats.DataModels;

internal record CompletionResponse {
    public required string Id { get; init; }
    public required string Model { get; init; }
    public required Choice[] Choices { get; init; }
    public required int Created { get; init; }
    public required string SystemFingerprint { get; init; }
    public required Usage Usage { get; init; }
}
