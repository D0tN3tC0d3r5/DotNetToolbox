namespace DotNetToolbox.ConsoleApplication.Application;

public abstract class ApplicationOptions<TOptions>
    : NamedOptions<TOptions>
    , IApplicationOptions
    where TOptions : ApplicationOptions<TOptions>, new() {
    public bool ClearScreenOnStart { get; set; }
}
