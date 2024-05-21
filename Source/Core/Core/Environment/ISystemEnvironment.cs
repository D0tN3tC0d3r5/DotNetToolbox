namespace DotNetToolbox.Environment;

public interface ISystemEnvironment {
    string Name { get; }
    IAssemblyDescriptor Assembly { get; }
    IDateTimeProvider DateTime { get; }
    IFileSystemAccessor FileSystemAccessor { get; }
    IGuidProvider Guid { get; }
    IInput ConsoleInput { get; }
    IOutput ConsoleOutput { get; }
}
