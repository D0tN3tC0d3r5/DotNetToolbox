namespace DotNetToolbox.Http;

public interface IHttpClientOptionsBuilder {
    IHttpClientOptionsBuilder SetBaseAddress(string baseAddress);
    IHttpClientOptionsBuilder SetResponseFormat(string responseFormat);
    IHttpClientOptionsBuilder AddCustomHeader(string key, string value);

    IHttpClientOptionsBuilder UseApiKeyAuthentication(Action<ApiKeyAuthenticationOptions> options);
    IHttpClientOptionsBuilder UseSimpleTokenAuthentication(Action<StaticTokenAuthenticationOptions> options);
    IHttpClientOptionsBuilder UseJsonWebTokenAuthentication(Action<JwtAuthenticationOptions> options);
    IHttpClientOptionsBuilder UseOAuth2TokenAuthentication(Action<OAuth2TokenAuthenticationOptions> options);
}