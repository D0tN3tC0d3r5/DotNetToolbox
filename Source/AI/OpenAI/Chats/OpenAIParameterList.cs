namespace DotNetToolbox.AI.OpenAI.Chats;

public record OpenAIParameterList {
    public Dictionary<string, OpenAIParameter> Properties { get; init; } = [];
    public string[] Required { get; init; } = [];
}
