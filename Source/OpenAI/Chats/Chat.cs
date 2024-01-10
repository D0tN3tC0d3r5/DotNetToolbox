namespace DotNetToolbox.OpenAI.Chats;
public record Chat {
    public Chat(string model)
        : this(new ChatOptions(model)) {
    }

    public Chat(ChatOptions? options = null) {
        Options = options ?? Options;
    }

    public string Id { get; init; } = Guid.NewGuid().ToString();
    public List<Message> Messages { get; } = [];
    public ChatOptions Options { get; } = new();
}
