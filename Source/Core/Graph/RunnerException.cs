namespace DotNetToolbox.Graph;

[Serializable]
internal class RunnerException(int errorCode, string message, Exception? innerException = null)
    : Exception(IsNotNullOrWhiteSpace(message), innerException) {
    public RunnerException(int errorCode = 1, Exception? innerException = null)
        : this(errorCode, "An error occurred while running the workflow.", innerException) {
        ErrorCode = errorCode;
    }

    public RunnerException(string message, Exception? innerException = null)
        : this(1, IsNotNullOrWhiteSpace(message), innerException) {
    }

    public int ErrorCode { get; } = errorCode;
}