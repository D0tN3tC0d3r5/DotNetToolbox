namespace DotNetToolbox.Singleton;

public class NamedOptions<TOptions>
    : HasDefault<TOptions>
    , INamedOptions<TOptions>
    where TOptions : NamedOptions<TOptions>, new() {
    private const string _suffix = "Options";
    private static readonly string _typeName = typeof(TOptions).Name;

    public static string SectionName
        => _typeName.EndsWith(_suffix)
               ? _typeName[.._suffix.Length]
               : _typeName;
}
