namespace DotNetToolbox.ConsoleApplication;

public class RunOnceApplicationOptions
    : RunOnceApplicationOptions<RunOnceApplicationOptions>;

public abstract class RunOnceApplicationOptions<TOptions>
    : ApplicationOptions<TOptions>
    where TOptions : RunOnceApplicationOptions<TOptions>, new();
