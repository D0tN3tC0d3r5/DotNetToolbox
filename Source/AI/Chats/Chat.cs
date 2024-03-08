namespace DotNetToolbox.AI.Chats;

public abstract class Chat<TOptions>
    : IChat
    where TOptions : class, IChatOptions, new() {
    protected Chat(TOptions? options = null) {
        Options = options ?? Options;
    }

    public string Id { get; } = Guid.NewGuid().ToString();
    public int TotalNumberOfTokens { get; set; }

    IChatOptions IChat.Options => Options;
    public TOptions Options { get; } = new();
    public List<Message> Messages { get; } = [];
}
