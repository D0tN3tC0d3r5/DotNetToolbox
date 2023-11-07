using Microsoft.Extensions.DependencyInjection;

namespace DotNetToolbox.Http.Extensions;

public sealed class ServiceCollectionExtensionsTests {
    [Fact]
    public void AddHttpClientProvider_AddServices() {
        // Arrange
        var services = new ServiceCollection();
        var configuration = Substitute.For<IConfiguration>();

        // Act
        services.AddHttpClientProvider(configuration);
    }
}