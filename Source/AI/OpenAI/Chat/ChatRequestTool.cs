namespace DotNetToolbox.AI.OpenAI.Chat;

public class ChatRequestTool(string type, ChatRequestToolFunctionCall function) {
    [JsonPropertyName("type")]
    public string Type { get; set; } = type;
    [JsonPropertyName("function")]
    public ChatRequestToolFunctionCall Function { get; set; } = function;
}
