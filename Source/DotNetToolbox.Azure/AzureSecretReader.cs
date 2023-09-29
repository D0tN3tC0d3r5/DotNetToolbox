using DotNetToolbox.Azure;

namespace DotNetToolbox;

public sealed class AzureSecretReader
    : IAzureSecretReader
{
    private readonly SecretClient? _client;

    public AzureSecretReader(IConfiguration configuration)
    {
        if (configuration.GetValue("UseLocalSecrets", false))
        {
            return;
        }

        var keyVaultUrl = IsNotNull(configuration["KeyVaultUrl"]);
        var credential = new DefaultAzureCredential();
        var keyVaultUri = new Uri(keyVaultUrl);
        _client = new(keyVaultUri, credential);
    }

    public TValue? GetSecretOrDefault<TValue>(string key, TValue? defaultValue = default)
    {
        IsNotNull(key);
        if (_client is null)
        {
            return defaultValue;
        }

        try
        {
            var secret = _client.GetSecret(key).Value;
            return (TValue)Convert.ChangeType(secret.Value, typeof(TValue));
        }
        catch (Exception)
        {
            return defaultValue;
        }
    }

    public string GetSecretOrKey(string key)
        => GetSecretOrDefault(key, key)!;
}
