namespace DotNetToolbox.Http;

public interface ITokenAcquirer
{
    Task<string> AcquireTokenAsync<TOptions>(TOptions options)
        where TOptions : ConfidentialHttpClientOptions;
}
