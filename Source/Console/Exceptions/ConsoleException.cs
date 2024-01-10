namespace DotNetToolbox.ConsoleApplication.Exceptions;
public class ConsoleException(int exitCode, string message, Exception? innerException = null)
    : Exception(message, innerException) {
    public const int DefaultErrorCode = 1;

    public ConsoleException(string message, Exception? innerException = null)
        : this(DefaultErrorCode, message, innerException) {
    }

    public int ExitCode { get; } = exitCode;
}
