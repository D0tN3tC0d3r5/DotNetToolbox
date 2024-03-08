namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIChatResponseToolCall(string id, string type, OpenAIChatResponseToolCallFunction function) {
    [JsonPropertyName("id")]
    public string Id { get; set; } = id;
    [JsonPropertyName("type")]
    public string Type { get; set; } = type;
    [JsonPropertyName("function")]
    public OpenAIChatResponseToolCallFunction Function { get; set; } = function;
}
