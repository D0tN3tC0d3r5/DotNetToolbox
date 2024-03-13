namespace DotNetToolbox.AI.OpenAI.Chat;

public class ChatTool(string name, Dictionary<string, ChatToolParameter>? parameters = null, string? description = null) {
    public string Name { get; set; } = name;
    public string? Description { get; set; } = description;
    public Dictionary<string, ChatToolParameter> Parameters { get; set; } = parameters ?? [];
}
