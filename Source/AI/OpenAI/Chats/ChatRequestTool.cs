namespace DotNetToolbox.AI.OpenAI.Chats;

public class ChatRequestTool(string type, ChatRequestToolFunctionCall function) {
    [JsonPropertyName("type")]
    public string Type { get; set; } = type;
    [JsonPropertyName("function")]
    public ChatRequestToolFunctionCall Function { get; set; } = function;
}
