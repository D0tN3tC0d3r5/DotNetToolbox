namespace DotNetToolbox.AI.Consumers;

public interface IResponseConsumer
    : IResponseAwaiter {
    void ResponseApproved(Guid chat, int? agent, Message response);
    bool VerifyResponse(Guid chat, int? agent, Message response);
}
