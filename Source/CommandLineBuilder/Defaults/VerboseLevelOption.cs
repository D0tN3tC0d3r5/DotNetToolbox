namespace DotNetToolbox.CommandLineBuilder.Defaults;

internal sealed class VerboseLevelOption() : Option<int>("verbose",
                                                         'v',
                                                         "Show verbose output.",
                                                         t => t.Writer.VerboseLevel = (OutputVerboseLevel)((Option<int>)t).Value);
