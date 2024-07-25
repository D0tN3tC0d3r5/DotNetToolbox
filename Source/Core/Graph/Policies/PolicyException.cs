namespace DotNetToolbox.Graph.Policies;

public class PolicyException
    : Exception {
    public PolicyException() : base() {
    }

    public PolicyException(string? message) : base(message) {
    }

    public PolicyException(string? message, Exception? innerException) : base(message, innerException) {
    }
}
