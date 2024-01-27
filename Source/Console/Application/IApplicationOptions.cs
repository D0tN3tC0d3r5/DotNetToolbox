namespace DotNetToolbox.ConsoleApplication.Application;

public interface IApplicationOptions {
    string Environment { get; }
    bool ClearScreenOnStart { get; }
}
