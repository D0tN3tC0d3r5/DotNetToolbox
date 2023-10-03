namespace DotNetToolbox.Http;

[ExcludeFromCodeCoverage]
internal class TokenAcquirer<T>
    : ITokenAcquirer
    where T : BaseAbstractAcquireTokenParameterBuilder<T> {
    private readonly BaseAbstractAcquireTokenParameterBuilder<T> _builder;

    public TokenAcquirer(BaseAbstractAcquireTokenParameterBuilder<T> builder) {
        _builder = builder;
    }

    public async Task<string> AcquireTokenAsync() {
        var token = await _builder.ExecuteAsync(CancellationToken.None);
        return token.CreateAuthorizationHeader();
    }
}
