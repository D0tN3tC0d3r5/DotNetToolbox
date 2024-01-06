namespace ConsoleApplication.Exceptions;
public class ConsoleException : Exception {
    public const int DefaultErrorCode = 1;

    public ConsoleException(string message, Exception? innerException = null)
        : this(DefaultErrorCode, message, innerException) {
    }

    public ConsoleException(int exitCode, string message, Exception? innerException = null)
        : base(message, innerException)
    {
        ExitCode = exitCode;
    }

    public int ExitCode { get; }
}
