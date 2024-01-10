namespace DotNetToolbox.OpenAI.Tools;

public record ParameterList {
    public Dictionary<string, Parameter> Properties { get; init; } = [];
    public string[] Required { get; init; } = [];
}
