namespace DotNetToolbox.AI.OpenAI.Chat;

public class OpenAIChatResponseToolRequest(string id, string type, OpenAIChatResponseFunctionCallRequest function) {
    [JsonPropertyName("id")]
    public string Id { get; set; } = id;
    [JsonPropertyName("type")]
    public string Type { get; set; } = type;
    [JsonPropertyName("function")]
    public OpenAIChatResponseFunctionCallRequest Function { get; set; } = function;
}
