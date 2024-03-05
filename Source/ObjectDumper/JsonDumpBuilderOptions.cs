namespace DotNetToolbox;

public record JsonDumpBuilderOptions : BasicDumpBuilderOptions {
    public override byte MaxDepth { get; set; } = 10;
}
