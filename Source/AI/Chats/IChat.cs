namespace DotNetToolbox.AI.Chats;

public interface IChat {
    string Id { get; }
    List<Message> Messages { get; }
    IChatOptions Options { get; }
    public int TotalNumberOfTokens { get; set; }
    Task<HttpResult> Submit(CancellationToken ct = default);
}
