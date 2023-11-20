namespace DotNetToolbox.CommandLineBuilder.Defaults;

internal sealed class NoColorFlag : Flag
{
    public NoColorFlag()
        : base("no-color", "Don't colorize output.", false, t => t.Writer.UseColors = false)
    {
    }
}
