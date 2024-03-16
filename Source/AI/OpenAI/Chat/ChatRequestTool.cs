namespace DotNetToolbox.AI.OpenAI.Chat;

public class ChatRequestTool(string type, RequestToolFunctionCall function) {
    [JsonPropertyName("type")]
    public string Type { get; set; } = type;
    [JsonPropertyName("function")]
    public RequestToolFunctionCall Function { get; set; } = function;
}
