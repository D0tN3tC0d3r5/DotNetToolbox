using DotNetToolbox.AI.Chats;

namespace Sophia.WebApp.Services;

public class UserProxy(ChatData chat, ResponseBuffer buffer) : IConsumer {
    public async Task ProcessResponse(string chatId, Message response, CancellationToken ct) {
        buffer.Message = response.Parts.Aggregate(string.Empty, (s, v) => s += v);
        buffer.ResponseReceived = true;
    }
}
