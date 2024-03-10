namespace DotNetToolbox.AI.Chats;

public interface IChatHandler : IChat {
    public int TotalNumberOfTokens { get; set; }
    Task<HttpResult> Submit(CancellationToken ct = default);
}
