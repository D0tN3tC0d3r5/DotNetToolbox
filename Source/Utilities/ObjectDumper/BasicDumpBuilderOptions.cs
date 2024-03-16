namespace DotNetToolbox;

public record BasicDumpBuilderOptions {
    public virtual byte MaxDepth { get; set; } = 1;
    public virtual bool Indented { get; set; } = true;
}
