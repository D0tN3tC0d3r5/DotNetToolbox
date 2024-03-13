namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIChatRequestTool(string type, OpenAIChatRequestToolFunctionCall function) {
    [JsonPropertyName("type")]
    public string Type { get; set; } = type;
    [JsonPropertyName("function")]
    public OpenAIChatRequestToolFunctionCall Function { get; set; } = function;
}
