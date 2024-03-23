namespace DotNetToolbox.AI.Consumers;

public interface IResponseConsumer
    : IResponseAwaiter {
    void ResponseApproved(string chatId, Message response);
    bool VerifyResponse(string chatId, Message response);
}
