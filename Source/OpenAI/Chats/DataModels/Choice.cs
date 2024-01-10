namespace DotNetToolbox.OpenAI.Chats.DataModels;

[JsonDerivedType(typeof(MessageChoice), typeDiscriminator: "message")]
[JsonDerivedType(typeof(DeltaChoice), typeDiscriminator: "delta")]
internal record Choice {
    public int Index { get; init; }
    public string? FinishReason { get; init; }
}
