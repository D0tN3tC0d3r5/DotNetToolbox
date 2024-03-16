namespace DotNetToolbox.AI.Anthropic.Chat;

public class ResponseMessage {
    [JsonPropertyName("content")]
    public required MessageContent[] Content { get; set; } = [];

    [JsonPropertyName("finish_reason")]
    public string StopReason { get; set; } = string.Empty;
}
