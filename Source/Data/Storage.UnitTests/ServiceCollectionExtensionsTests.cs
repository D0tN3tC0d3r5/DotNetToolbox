namespace DotNetToolbox.Data;

public class ServiceCollectionExtensionsTests {
    [Fact]
    public void AddRepositoryStrategyProvider_AddsProvider() {
        var services = new ServiceCollection();

        services.AddRepositoryStrategyProvider();

        var provider = services.BuildServiceProvider();

        var service = provider.GetService<IRepositoryStrategyProvider>();

        service.Should().BeOfType<RepositoryStrategyProvider>();
    }

    [Fact]
    public void AddRepositoryStrategyProvider_WithConfiguration_AddsProvider() {
        var services = new ServiceCollection();

        services.AddRepositoryStrategyProvider(p => p.TryAdd<InMemoryValueObjectRepositoryStrategy<string>>());

        var provider = services.BuildServiceProvider();

        var service = provider.GetService<IRepositoryStrategyProvider>();

        service.Should().BeOfType<RepositoryStrategyProvider>();
    }
}
