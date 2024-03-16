namespace DotNetToolbox.DependencyInjection;

public class ServiceCollectionExtensionsTests {
    [Fact]
    public void AddEnvironment_RegisterHandlers() {
        var services = new ServiceCollection();
        var result = services.AddEnvironment();
        using var provider = result.BuildServiceProvider();
        var environment = provider.GetRequiredService<IEnvironment>();

        environment.Name.Should().BeEmpty();
        environment.Assembly.Should().NotBeNull();
        environment.DateTime.Should().NotBeNull();
        environment.Output.Should().NotBeNull();
        environment.Input.Should().NotBeNull();
        environment.FileSystem.Should().NotBeNull();

        result.Should().BeSameAs(services);
    }

    [Fact]
    public void AddEnvironment_WithNamedEnvironment_RegisterHandlers() {
        var services = new ServiceCollection();
        var result = services.AddEnvironment("Development");
        var provider = services.BuildServiceProvider();
        var environment = provider.GetRequiredService<IEnvironment>();

        environment.Name.Should().Be("Development");
        environment.Assembly.Should().NotBeNull();
        environment.DateTime.Should().NotBeNull();
        environment.Output.Should().NotBeNull();
        environment.Input.Should().NotBeNull();
        environment.FileSystem.Should().NotBeNull();

        result.Should().BeSameAs(services);
    }

    [Fact]
    public void AddEnvironment_WithFakeEnvironment_RegisterHandlers() {
        var services = new ServiceCollection();
        var environment = Substitute.For<IEnvironment>();
        environment.Name.Returns("Development");
        services.AddEnvironment(environment);
        var provider = services.BuildServiceProvider();

        var env = provider.GetRequiredService<IEnvironment>();

        env.Name.Should().Be("Development");
        env.Assembly.Should().NotBeNull();
        env.DateTime.Should().NotBeNull();
        env.Output.Should().NotBeNull();
        env.Input.Should().NotBeNull();
        env.FileSystem.Should().NotBeNull();
    }

    [Fact]
    public void AddEnvironment_WithDefaultEnvironment_RegisterHandlers() {
        var services = new ServiceCollection();
        services.AddAssemblyDescriptor();
        services.AddDateTimeProvider();
        services.AddGuidProvider();
        services.AddFileSystem();
        services.AddInput();
        services.AddOutput();
        var environment = new Environment(services.BuildServiceProvider());
        services.AddEnvironment(environment);
        var provider = services.BuildServiceProvider();

        var env = provider.GetRequiredService<IEnvironment>();

        env.Name.Should().BeEmpty();
        env.Assembly.Should().NotBeNull();
        env.DateTime.Should().NotBeNull();
        env.Output.Should().NotBeNull();
        env.Input.Should().NotBeNull();
        env.FileSystem.Should().NotBeNull();
    }
}
