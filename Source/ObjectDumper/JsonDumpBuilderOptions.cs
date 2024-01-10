namespace DotNetToolbox;

public record JsonDumpBuilderOptions {
    public byte MaxDepth { get; set; } = 10;
    public bool Indented { get; set; } = true;
}
