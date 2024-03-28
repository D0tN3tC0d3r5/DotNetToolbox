namespace DotNetToolbox.Http;

public interface IHttpClientProvider {
    string Name { get; }
    HttpClientOptions Options { get; }
    HttpClient GetHttpClient();
    void Authorize(string value, DateTimeOffset? expiresOn = null);
    void ExtendAuthorizationUntil(DateTimeOffset expiresOn);
    void RevokeAuthorization();
}
