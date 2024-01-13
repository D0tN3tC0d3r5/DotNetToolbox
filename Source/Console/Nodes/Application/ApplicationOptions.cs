namespace DotNetToolbox.ConsoleApplication.Nodes.Application;

public abstract class ApplicationOptions<TOptions>
    : NamedOptions<TOptions>
    , IApplicationOptions
    where TOptions : ApplicationOptions<TOptions>, new() {
    public string Environment { get; set; } = string.Empty;
    public bool ClearScreenOnStart { get; set; }
}
