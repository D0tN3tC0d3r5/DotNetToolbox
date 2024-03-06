namespace DotNetToolbox.AI.Chats;

public abstract class Chat<TOptions>
    : IChat<TOptions>
    where TOptions : ChatOptions, new() {
    protected Chat(string userName, TOptions? options = null) {
        UserName = userName;
        Options = options ?? Options;
    }

    public string Id { get; } = Guid.NewGuid().ToString();
    public string UserName { get; }
    public TOptions Options { get; } = new();
    public int TotalNumberOfTokens { get; set; }
}
