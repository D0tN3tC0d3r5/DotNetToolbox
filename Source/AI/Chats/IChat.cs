namespace DotNetToolbox.AI.Chats;

public interface IChat {
    string Id { get; }
    string UserName { get; }
    public int TotalNumberOfTokens { get; set; }
}

public interface IChat<out TOptions> : IChat
    where TOptions : ChatOptions, new() {
    TOptions Options { get; }
}
