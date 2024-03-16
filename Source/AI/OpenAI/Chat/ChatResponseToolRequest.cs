namespace DotNetToolbox.AI.OpenAI.Chat;

public class ChatResponseToolRequest(string id, string type, ChatResponseFunctionCallRequest function) {
    [JsonPropertyName("id")]
    public string Id { get; set; } = id;
    [JsonPropertyName("type")]
    public string Type { get; set; } = type;
    [JsonPropertyName("function")]
    public ChatResponseFunctionCallRequest Function { get; set; } = function;
}
