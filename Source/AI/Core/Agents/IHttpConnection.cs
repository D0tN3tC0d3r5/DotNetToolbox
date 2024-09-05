namespace DotNetToolbox.AI.Agents;

public interface IHttpConnection {
    HttpConnectionSettings Settings { get; }
    Task<HttpResult> SendRequest(IJob job, IChat chat, CancellationToken ct = default);
}
