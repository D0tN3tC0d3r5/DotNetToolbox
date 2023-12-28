namespace DotNetToolbox.OpenAI.Chats.DataModels;

internal record Prompt : Message {
    [JsonPropertyName("role")]
    public override MessageType Type { get; init; }
}
