namespace DotNetToolbox.AI.Anthropic.Chat;

public class RequestMessage {
    public RequestMessage(object content) {
        switch (content) {
            case Message c:
                Role = c.Role;
                Content = c.Parts.Length == 1
                              ? (string)c.Parts[0].Value
                              : c.Parts.ToArray(p => new MessageContent(p.Value));
                break;
            default:
                throw new NotSupportedException();
        }
    }
    [JsonPropertyName("role")]
    public string Role { get; init; }
    [JsonPropertyName("content")]
    public object Content { get; init; }
}
