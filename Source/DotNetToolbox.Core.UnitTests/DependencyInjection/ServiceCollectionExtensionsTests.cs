namespace System.DependencyInjection;

public class ServiceCollectionExtensionsTests {
    [Fact]
    public void AddCoreUtilities_RegisterHandlers() {
        var services = new ServiceCollection();

        var result = services.AddSystemUtilities();

        _ = result.Should().BeSameAs(services);
    }
}
