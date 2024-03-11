namespace DotNetToolbox.AI.Agents;

public class ResponsePackage(string chatId, Message message, CancellationToken token) {
    public string ChatId { get; } = chatId;
    public Message Message { get; } = message;
    public CancellationToken Token { get; } = token;
}
