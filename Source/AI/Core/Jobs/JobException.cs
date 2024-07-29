namespace DotNetToolbox.AI.Jobs;

public class JobException : Exception {
    public JobException() {
    }

    public JobException(string message, Exception? innerException = null)
        : base(message, innerException) {
    }
}
