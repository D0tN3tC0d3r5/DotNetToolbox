namespace DotNetToolbox.AI.OpenAI.Chats;

public class ChatResponseToolRequest(string id, ChatResponseFunctionCallRequest function) {
    [JsonPropertyName("id")]
    public string Id { get; set; } = id;
    [JsonPropertyName("type")]
    public MessageRole Type { get; } = MessageRole.Tool;
    [JsonPropertyName("function")]
    public ChatResponseFunctionCallRequest Function { get; set; } = function;
}
