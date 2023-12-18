namespace DotNetToolbox;

public class DumpBuilderOptions {
    public Layout Layout { get; set; } = Layout.TypedJson;
    public bool UseTabs { get; set; }
    public int IndentSize { get; set; } = 4;
    public byte MaxLevel { get; set; } = 5;
    public bool Indented { get; set; } = true;
    public bool ShowListIndexes { get; set; }
    public bool ShowTypeFullName { get; set; }
    public CultureInfo Culture { get; set; } = CultureInfo.CurrentCulture;

    public Dictionary<Type, Func<object, string>> CustomFormatters { get; set; } = new();
}
