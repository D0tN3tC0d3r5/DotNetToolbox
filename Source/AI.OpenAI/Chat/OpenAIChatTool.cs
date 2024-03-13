namespace DotNetToolbox.AI.OpenAI.Chat;

public class OpenAIChatTool(string name, Dictionary<string, OpenAIChatToolParameter>? parameters = null, string? description = null) {
    public string Name { get; set; } = name;
    public string? Description { get; set; } = description;
    public Dictionary<string, OpenAIChatToolParameter> Parameters { get; set; } = parameters ?? [];
}
