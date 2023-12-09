namespace DotNetToolbox.CommandLineBuilder.Defaults;

internal sealed class HelpFlag : Flag
{
    public HelpFlag()
        : base("help", 'h', "Show this help information and exit.", true, t => t.Writer.WriteHelp(t.Parent!))
    {
    }
}