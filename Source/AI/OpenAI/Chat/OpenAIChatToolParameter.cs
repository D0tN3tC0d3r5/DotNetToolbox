namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIChatToolParameter(string type, string[]? options = null, bool isRequired = false, string? description = null) {
    public required string Type { get; set; } = type;
    public string? Description { get; set; } = description;
    public string[]? Options { get; set; } = options;
    public bool IsRequired { get; set; } = isRequired;
}
