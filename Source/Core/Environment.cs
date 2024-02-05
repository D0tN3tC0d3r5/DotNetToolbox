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
    public virtual IAssemblyDescriptor Assembly { get; } = services.GetRequiredKeyedService<IAssemblyDescriptor>(name ?? string.Empty);
    public virtual IOutput Output { get; } = services.GetRequiredKeyedService<IOutput>(name ?? string.Empty);
    public virtual IInput Input { get; } = services.GetRequiredKeyedService<IInput>(name ?? string.Empty);
    public virtual IDateTimeProvider DateTime { get; } = services.GetRequiredKeyedService<IDateTimeProvider>(name ?? string.Empty);
    public virtual IGuidProvider Guid { get; } = services.GetRequiredKeyedService<IGuidProvider>(name ?? string.Empty);
    public virtual IFileSystem FileSystem { get; } = services.GetRequiredKeyedService<IFileSystem>(name ?? string.Empty);
}
