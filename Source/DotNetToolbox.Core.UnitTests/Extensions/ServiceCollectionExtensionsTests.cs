namespace DotNetToolbox.Extensions;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddDomainHandlers_RegisterHandlers()
    {
        var services = new ServiceCollection();

        var result = services.AddCoreUtilities();

        result.Should().BeSameAs(services);
    }
}
