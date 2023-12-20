namespace DotNetToolbox;

public class DumpBuilderOptions {
    public Layout Layout { get; set; } = Layout.Typed;
    public bool UseTabs { get; set; }
    public int IndentSize { get; set; } = 4;
    public byte MaxLevel { get; set; } = 5;
    public bool Indented { get; set; } = true;
    public bool UseFullNames { get; set; }

    public Dictionary<Type, Func<object?, string>> CustomFormatters { get; set; } = [];
}
