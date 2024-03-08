namespace DotNetToolbox.AI.Anthropic.Chats;

public class AnthropicRequestMessage {
    public AnthropicRequestMessage(object content) {
        switch (content) {
            case Message c:
                Role = c.Role;
                Content = c.Parts.ToArray(p => new AnthropicMessageContent(p));
                break;
            default:
                throw new NotSupportedException();
        }
    }
    [JsonPropertyName("role")]
    public string Role { get; init; }
    [JsonPropertyName("content")]
    public object Content { get; init; }
    [JsonPropertyName("finish_reason")]
    public string? StopReason { get; set; }
}
