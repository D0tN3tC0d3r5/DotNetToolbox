namespace DotNetToolbox;

public class DumpBuilderOptions : JsonDumpBuilderOptions {
    public bool UseTabs { get; set; }
    public int IndentSize { get; set; } = 4;
    public bool UseFullNames { get; set; }

    public Dictionary<Type, Func<object?, string>> CustomFormatters { get; set; } = [];
}
