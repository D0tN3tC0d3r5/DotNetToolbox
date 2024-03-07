namespace DotNetToolbox.AI.Chats;

public interface IChat {
    string Id { get; }
    string UserName { get; }
    IEnumerable<IMessage> Messages { get; }
    IChatOptions Options { get; }
    public int TotalNumberOfTokens { get; set; }
}

public interface IChat<out TOptions, TMessage> : IChat
    where TOptions : IChatOptions, new()
    where TMessage : IMessage {
    new TOptions Options { get; }
    new List<TMessage> Messages { get; }
}
