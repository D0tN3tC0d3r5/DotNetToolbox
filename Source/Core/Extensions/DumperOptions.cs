namespace DotNetToolbox.Extensions;

public class DumperOptions {
    public Layout Layout { get; set; } = Layout.Json;
    public bool IndentOutput { get; set; } = true;
    public bool ShowListIndexes { get; set; }
    public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;
    public bool UseTabs { get; set; }
    public int IndentSize { get; set; } = 2;
    public byte MaxLevel { get; set; } = 20;
    public NewLineType NewLineType { get; set; }

    public Dictionary<Type, Func<object, string>> CustomFormatters { get; set; } = new();
}
