namespace DotNetToolbox.ConsoleApplication.Exceptions;
public class ConsoleException : Exception {
    public ConsoleException(string message, Exception? innerException = null)
        : this(1, message, innerException) {
    }

    public ConsoleException(int exitCode, string message, Exception? innerException = null)
        : base(message, innerException)
    {
        ExitCode = exitCode;
    }

    public int ExitCode { get; }
}
