namespace DotNetToolbox.ConsoleApplication.Exceptions;
[SuppressMessage("Roslynator", "RCS1194:Implement exception constructors", Justification = "All required are constructors implemented.")]
public class ConsoleException(string message, Exception? innerException = null) : Exception(IsNotNullOrEmpty(message), innerException) {
    public const string DefaultMessage = "An error occurred while executing the application.";

    public ConsoleException(int exitCode = Application.DefaultErrorCode, string? message = null, Exception? innerException = null)
        : this(message ?? DefaultMessage, innerException) {
        ExitCode = exitCode;
    }

    public int ExitCode { get; }
}
