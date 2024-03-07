namespace DotNetToolbox.AI.Chats;

public abstract class Chat<TOptions, TMessage>
    : IChat<TOptions, TMessage>
    where TOptions : class, IChatOptions, new()
    where TMessage : class, IMessage {
    protected Chat(string userName, TOptions? options = null) {
        UserName = userName;
        Options = options ?? Options;
    }

    public string Id { get; } = Guid.NewGuid().ToString();
    public string UserName { get; }
    public int TotalNumberOfTokens { get; set; }

    IChatOptions IChat.Options => Options;
    public TOptions Options { get; } = new();

    IEnumerable<IMessage> IChat.Messages => Messages;
    public List<TMessage> Messages { get; } = [];
}
