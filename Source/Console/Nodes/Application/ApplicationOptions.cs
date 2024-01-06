namespace ConsoleApplication.Nodes.Application;

public abstract record ApplicationOptions<TOptions>
    : IApplicationOptions
    where TOptions : ApplicationOptions<TOptions> {
    public string Environment { get; init; } = string.Empty;
    public bool ClearScreenOnStart { get; init; }
}
