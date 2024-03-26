namespace DotNetToolbox.Http;

public interface IHttpClientProvider {
    string Name { get; }
    HttpClient GetHttpClient();
    void Authorize(string value, DateTimeOffset? expiresOn = null);
    void ExtendAuthorizationUntil(DateTimeOffset expiresOn);
    void RevokeAuthorization();
}
