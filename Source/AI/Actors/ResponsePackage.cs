namespace DotNetToolbox.AI.Actors;

public class ResponsePackage(string chatId, Message message) {
    public string ChatId { get; } = chatId;
    public Message Message { get; } = message;
}
