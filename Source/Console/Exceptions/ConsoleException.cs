namespace DotNetToolbox.ConsoleApplication.Exceptions;
[SuppressMessage("Roslynator", "RCS1194:Implement exception constructors", Justification = "All required are constructors implemented.")]
public class ConsoleException(int exitCode = Application.DefaultErrorCode, string? message = null, Exception? innerException = null)
    : Exception(message ?? DefaultMessage, innerException) {
    public const string DefaultMessage = "An error occurred while executing the application.";

    public ConsoleException(string message, Exception? innerException = null)
        : this(Application.DefaultErrorCode, IsNotNullOrEmpty(message), innerException) {
    }

    public int ExitCode { get; } = exitCode;
}
