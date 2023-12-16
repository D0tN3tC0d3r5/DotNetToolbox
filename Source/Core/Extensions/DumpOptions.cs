namespace DotNetToolbox.Extensions;

public class DumpOptions {
    public Layout Layout { get; set; } = Layout.Json;
    public bool IndentOutput { get; set; } = true;
    public bool ShowListIndexes { get; set; }
    public bool ShowType { get; set; } = true;
    public bool ShowTypeFullName { get; set; }
    public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;
    public bool UseTabs { get; set; }
    public int IndentSize { get; set; } = 2;
    public byte MaxLevel { get; set; } = 5;

    public Dictionary<Type, Func<object, string>> CustomFormatters { get; set; } = new();
}
