namespace DotNetToolbox;

public interface IEnvironment {
    string Name { get; }
    IAssemblyDescriptor Assembly { get; }
    IDateTimeProvider DateTime { get; }
    IFileSystem FileSystem { get; }
    IGuidProvider Guid { get; }
    IInput Input { get; }
    IOutput Output { get; }
}

public class Environment(IServiceProvider services, string? name = null) : IEnvironment {
    public virtual string Name { get; } = name ?? string.Empty;
    public virtual IAssemblyDescriptor Assembly { get; } = services.GetRequiredService<IAssemblyDescriptor>();
    public virtual IOutput Output { get; } = services.GetRequiredService<IOutput>();
    public virtual IInput Input { get; } = services.GetRequiredService<IInput>();
    public virtual IDateTimeProvider DateTime { get; } = services.GetRequiredService<IDateTimeProvider>();
    public virtual IGuidProvider Guid { get; } = services.GetRequiredService<IGuidProvider>();
    public virtual IFileSystem FileSystem { get; } = services.GetRequiredService<IFileSystem>();
}
