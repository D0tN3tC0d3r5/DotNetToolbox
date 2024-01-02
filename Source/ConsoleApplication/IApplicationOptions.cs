namespace DotNetToolbox.ConsoleApplication;

public interface IApplicationOptions {
    string? Environment { get; }
    string? Name { get; }
    string? Version { get; }
    string? Description { get; }
}
