using DotNetToolbox.Environment;

namespace DotNetToolbox.DependencyInjection;

public class ServiceCollectionExtensionsTests {
    [Fact]
    public void AddEnvironment_RegisterHandlers() {
        var services = new ServiceCollection();
        var result = services.SetEnvironment();
        using var provider = result.BuildServiceProvider();
        var environment = provider.GetRequiredService<ISystemEnvironment>();

        environment.Name.Should().BeEmpty();
        environment.Assembly.Should().NotBeNull();
        environment.DateTime.Should().NotBeNull();
        environment.ConsoleOutput.Should().NotBeNull();
        environment.ConsoleInput.Should().NotBeNull();
        environment.FileSystemAccessor.Should().NotBeNull();

        result.Should().BeSameAs(services);
    }

    [Fact]
    public void AddEnvironment_WithNamedEnvironment_RegisterHandlers() {
        var services = new ServiceCollection();
        var result = services.SetEnvironment("Development");
        var provider = services.BuildServiceProvider();
        var environment = provider.GetRequiredService<ISystemEnvironment>();

        environment.Name.Should().Be("Development");
        environment.Assembly.Should().NotBeNull();
        environment.DateTime.Should().NotBeNull();
        environment.ConsoleOutput.Should().NotBeNull();
        environment.ConsoleInput.Should().NotBeNull();
        environment.FileSystemAccessor.Should().NotBeNull();

        result.Should().BeSameAs(services);
    }

    [Fact]
    public void AddEnvironment_WithFakeEnvironment_RegisterHandlers() {
        var services = new ServiceCollection();
        var environment = Substitute.For<ISystemEnvironment>();
        environment.Name.Returns("Development");
        services.SetEnvironment(environment);
        var provider = services.BuildServiceProvider();

        var env = provider.GetRequiredService<ISystemEnvironment>();

        env.Name.Should().Be("Development");
        env.Assembly.Should().NotBeNull();
        env.DateTime.Should().NotBeNull();
        env.ConsoleOutput.Should().NotBeNull();
        env.ConsoleInput.Should().NotBeNull();
        env.FileSystemAccessor.Should().NotBeNull();
    }

    [Fact]
    public void AddEnvironment_WithDefaultEnvironment_RegisterHandlers() {
        var services = new ServiceCollection();
        services.AddAssemblyDescriptor();
        services.SetDateTimeProvider();
        services.SetGuidProvider();
        services.SetFileSystemAccessor();
        services.SetConsoleInput();
        services.SetConsoleOutput();
        var environment = new SystemEnvironment(services.BuildServiceProvider());
        services.SetEnvironment(environment);
        var provider = services.BuildServiceProvider();

        var env = provider.GetRequiredService<ISystemEnvironment>();

        env.Name.Should().BeEmpty();
        env.Assembly.Should().NotBeNull();
        env.DateTime.Should().NotBeNull();
        env.ConsoleOutput.Should().NotBeNull();
        env.ConsoleInput.Should().NotBeNull();
        env.FileSystemAccessor.Should().NotBeNull();
    }
}
