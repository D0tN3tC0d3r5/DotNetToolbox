namespace DotNetToolbox.Http;

[ExcludeFromCodeCoverage]
internal class TokenAcquirer
    : ITokenAcquirer
{
    public async Task<string> AcquireTokenAsync<TOptions>(TOptions options)
        where TOptions : ConfidentialHttpClientOptions
    {
        var app = ConfidentialClientApplicationBuilder
                 .Create(options.ClientId)
                 .WithAuthority(options.Authority)
                 .WithClientSecret(options.ClientSecret)
                 .Build();

        var acquireToken = app.AcquireTokenForClient(options.Scopes);
        var token = await acquireToken.ExecuteAsync(CancellationToken.None);
        return token.CreateAuthorizationHeader();
    }
}
