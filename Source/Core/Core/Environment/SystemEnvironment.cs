namespace DotNetToolbox.Environment;

public class SystemEnvironment(IServiceProvider services, string? name = null) : ISystemEnvironment {
    public virtual string Name { get; } = name ?? string.Empty;
    public virtual IAssemblyDescriptor Assembly { get; } = services.GetRequiredService<IAssemblyDescriptor>();
    public virtual IOutput ConsoleOutput { get; } = services.GetRequiredService<IOutput>();
    public virtual IInput ConsoleInput { get; } = services.GetRequiredService<IInput>();
    public virtual IDateTimeProvider DateTime { get; } = services.GetRequiredService<IDateTimeProvider>();
    public virtual IGuidProvider Guid { get; } = services.GetRequiredService<IGuidProvider>();
    public virtual IFileSystemAccessor FileSystemAccessor { get; } = services.GetRequiredService<IFileSystemAccessor>();
}
