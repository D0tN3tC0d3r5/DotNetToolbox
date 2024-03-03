namespace DotNetToolbox.OpenAI.DataModels;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "object")]
[JsonDerivedType(typeof(MessageResponse), typeDiscriminator: "chat.completion")]
[JsonDerivedType(typeof(DeltaResponse), typeDiscriminator: "chat.completion.chunk")]
internal record CompletionResponse {
    public required string Id { get; init; }
    public string? Model { get; init; }
    public int Created { get; init; }
    public string? SystemFingerprint { get; init; }
    public Usage? Usage { get; init; }
}
