namespace DotNetToolbox.OpenAI.Chats;
public record Chat {
    public string Id { get; init; } = Guid.NewGuid().ToString();
    public List<Message> Messages { get; init; } = [];
    public required ChatOptions Options { get; init; }
}
