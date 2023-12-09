namespace DotNetToolbox.CommandLineBuilder.Defaults;

internal sealed class VerboseLevelOption : Option<int> {
    public VerboseLevelOption()
        : base("verbose", 'v', "Show verbose output.", t => t.Writer.VerboseLevel = (OutputVerboseLevel)((Option<int>)t).Value) {
    }
}
