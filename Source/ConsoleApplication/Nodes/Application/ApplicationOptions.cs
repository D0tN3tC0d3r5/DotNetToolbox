namespace DotNetToolbox.ConsoleApplication.Nodes.Application;

public record ApplicationOptions<TOptions>
    : IApplicationOptions
    where TOptions : ApplicationOptions<TOptions> {
    public string Environment { get; init; } = string.Empty;
    public bool ClearScreenOnStart { get; init; }
}

public record ApplicationOptions
    : ApplicationOptions<ApplicationOptions> {
}
