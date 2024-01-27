namespace DotNetToolbox;

public interface IAssemblyDescriptor {
    string Name { get; }
    string FullName { get; }
    Version Version { get; }
    string RuntimeVersion { get; }
    CultureInfo? CultureInfo { get; }
    string? PublicKey { get; }
    string? PublicKeyToken { get; }
    string Location { get; }
    MethodInfo? EntryPoint { get; }
    Module ManifestModule { get; }
    IEnumerable<Attribute> GetCustomAttributes();
    IEnumerable<Attribute> GetCustomAttributes(Type attributeType);
    IEnumerable<Attribute> GetCustomAttributes<TAttribute>()
        where TAttribute : Attribute;
    TAttribute? GetCustomAttribute<TAttribute>()
        where TAttribute : Attribute;
    Attribute? GetCustomAttribute(Type attributeType);
    IEnumerable<TypeInfo> DefinedTypes { get; }
    IEnumerable<Type> ExportedTypes { get; }
    IEnumerable<Module> Modules { get; }
}

[ExcludeFromCodeCoverage(Justification = "Thin wrapper for Assembly functionality.")]
public class AssemblyDescriptor : IAssemblyDescriptor {
    private readonly Assembly _assembly;

    internal AssemblyDescriptor(Assembly assembly) {
        _assembly = assembly;
    }

    public virtual string Name => _assembly.GetName().Name!;
    public virtual string FullName => _assembly.FullName!;
    public virtual Version Version => _assembly.GetName().Version!;
    public virtual CultureInfo? CultureInfo => _assembly.GetName().CultureInfo;
    public virtual string? PublicKey {
        get {
            var bytes = _assembly.GetName().GetPublicKey();
            return bytes is null ? null : Encoding.UTF8.GetString(bytes);
        }
    }
    public virtual string? PublicKeyToken {
        get {
            var bytes = _assembly.GetName().GetPublicKeyToken();
            return bytes is null ? null : Encoding.UTF8.GetString(bytes);
        }
    }

    public virtual string RuntimeVersion => _assembly.ImageRuntimeVersion;
    public virtual string Location => _assembly.Location;
    public virtual MethodInfo? EntryPoint => _assembly.EntryPoint;
    public virtual Module ManifestModule => _assembly.ManifestModule;
    public virtual IEnumerable<Attribute> GetCustomAttributes()
        => _assembly.GetCustomAttributes();
    public virtual IEnumerable<Attribute> GetCustomAttributes(Type attributeType)
        => _assembly.GetCustomAttributes(attributeType);
    public virtual IEnumerable<Attribute> GetCustomAttributes<TAttribute>()
        where TAttribute : Attribute
        => _assembly.GetCustomAttributes<TAttribute>();
    public virtual TAttribute? GetCustomAttribute<TAttribute>()
        where TAttribute : Attribute
        => _assembly.GetCustomAttribute<TAttribute>();
    public virtual Attribute? GetCustomAttribute(Type attributeType)
        => _assembly.GetCustomAttribute(attributeType);
    public virtual IEnumerable<TypeInfo> DefinedTypes => _assembly.DefinedTypes;
    public virtual IEnumerable<Type> ExportedTypes => _assembly.ExportedTypes;
    public virtual IEnumerable<Module> Modules => _assembly.Modules;
}
