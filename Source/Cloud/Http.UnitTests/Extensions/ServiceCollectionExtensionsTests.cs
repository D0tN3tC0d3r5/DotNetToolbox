namespace DotNetToolbox.Http.Extensions;

public sealed class ServiceCollectionExtensionsTests {
    [Fact]
    public void AddHttpClientProviderFactory_AddServices() {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddHttpClientProviderFactory();
    }

    [Fact]
    public void AddHttpClientProvider_AddServices() {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddHttpClientProvider();
    }
}
