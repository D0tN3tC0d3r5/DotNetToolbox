namespace DotNetToolbox.Azure;

public class AzureSecretReaderTests {
    private SecretClient? _secretClient;

    private AzureSecretReader CreateAzureSecretReader(bool useLocalSecrets) {
        var configuration = Substitute.For<IConfiguration>();
        var configurationSection = Substitute.For<IConfigurationSection>();
        configurationSection.Value.Returns(useLocalSecrets.ToString());
        configuration.GetSection("UseLocalSecrets").Returns(configurationSection);
        configuration["KeyVaultUrl"].Returns("https://keyVaultName.vault.azure.net/");

        var reader = new AzureSecretReader(configuration);

        if (useLocalSecrets) {
            return reader;
        }

        _secretClient = Substitute.For<SecretClient>();
        var clientField = typeof(AzureSecretReader).GetField(
            "_client",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;

        clientField.SetValue(reader, _secretClient);

        return reader;

    }

    [Fact]
    public void GetSecretOrDefault_WhenUsingLocalSecrets_ReturnsDefault() {
        // Arrange
        var secretName = "SecretName";
        var defaultValue = "This is a secret value";
        var azureSecretReader = CreateAzureSecretReader(true);

        // Act
        var result = azureSecretReader.GetSecretOrDefault(secretName, defaultValue);

        // Assert
        result.Should().Be(defaultValue);
    }

    [Fact]
    public void GetSecretOrDefault_WhenValueExists_ReturnsValue() {
        // Arrange
        var secretName = "SecretName";
        var secretValue = "This is a secret value";
        var azureSecretReader = CreateAzureSecretReader(false);

        var secret = new KeyVaultSecret(secretName, secretValue);
        var response = Substitute.For<global::Azure.Response<KeyVaultSecret>>();
        response.Value.Returns(secret);
        _secretClient!.GetSecret(secretName).Returns(response);

        // Act
        var result = azureSecretReader.GetSecretOrDefault<string>(secretName);

        // Assert
        result.Should().Be(secretValue);
    }

    [Fact]
    public void GetSecretOrDefault_WhenValueDoesNotExist_ReturnsDefaultValue() {
        // Arrange
        var secretName = "SecretName";
        var defaultValue = "This is a default value";
        var azureSecretReader = CreateAzureSecretReader(false);
        _secretClient!.GetSecret(secretName).Throws<Exception>();

        // Act
        var result = azureSecretReader.GetSecretOrDefault(secretName, defaultValue);

        // Assert
        result.Should().Be(defaultValue);
    }

    [Fact]
    public void GetSecretOrKey_WhenUsingLocalSecrets_ReturnsKey() {
        // Arrange
        var secretName = "SecretName";
        var azureSecretReader = CreateAzureSecretReader(true);

        // Act
        var result = azureSecretReader.GetSecretOrKey(secretName);

        // Assert
        result.Should().Be(secretName);
    }

    [Fact]
    public void GetSecretOrKey_WhenValueExists_ReturnsValue() {
        // Arrange
        var secretName = "SecretName";
        var secretValue = "This is a secret value";
        var azureSecretReader = CreateAzureSecretReader(false);

        var secret = new KeyVaultSecret(secretName, secretValue);
        var response = Substitute.For<global::Azure.Response<KeyVaultSecret>>();
        response.Value.Returns(secret);
        _secretClient!.GetSecret(secretName).Returns(response);

        // Act
        var result = azureSecretReader.GetSecretOrKey(secretName);

        // Assert
        result.Should().Be(secretValue);
    }

    [Fact]
    public void GetSecretOrKey_WhenValueDoesNotExist_ReturnsKey() {
        // Arrange
        var secretName = "SecretName";
        var azureSecretReader = CreateAzureSecretReader(false);

        _secretClient!.GetSecret(secretName).Throws<Exception>();

        // Act
        var result = azureSecretReader.GetSecretOrKey(secretName);

        // Assert
        result.Should().Be(secretName);
    }
}
