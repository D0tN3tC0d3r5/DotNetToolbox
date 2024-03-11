namespace DotNetToolbox.AI.Agents;

public class ResponsePackage(string chatId, Message message) {
    public string ChatId { get; } = chatId;
    public Message Message { get; } = message;
}
