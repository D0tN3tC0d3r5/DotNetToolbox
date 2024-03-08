namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIChatTool(string id, string name, Dictionary<string, OpenAIChatToolParameter>? parameters = null, string? description = null) {
    public string Id { get; set; } = id;
    public string Name { get; set; } = name;
    public Dictionary<string, OpenAIChatToolParameter> Parameters { get; set; } = parameters ?? [];
    public string? Description { get; set; } = description;
}
