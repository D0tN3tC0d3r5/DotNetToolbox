namespace DotNetToolbox.DependencyInjection;

public class ServiceCollectionExtensionsTests {
    [Fact]
    public void AddCoreUtilities_RegisterHandlers() {
        var services = new ServiceCollection();

        var result = services.AddSystemUtilities();

        result.Should().BeSameAs(services);
    }
}
