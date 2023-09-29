namespace System.DependencyInjection;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddCoreUtilities_RegisterHandlers()
    {
        var services = new ServiceCollection();

        var result = services.AddCoreUtilities();

        result.Should().BeSameAs(services);
    }
}
