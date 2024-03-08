namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIChatRequestToolCall(string id, OpenAIChatRequestToolCallFunction function) {
    [JsonPropertyName("id")]
    public string Id { get; set; } = id;

    [JsonPropertyName("type")]
    public string Type { get; set; } = "function";
    [JsonPropertyName("function")]
    public OpenAIChatRequestToolCallFunction Function { get; set; } = function;
}
