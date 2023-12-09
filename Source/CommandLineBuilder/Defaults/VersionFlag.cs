namespace DotNetToolbox.CommandLineBuilder.Defaults;

internal sealed class VersionFlag() : Flag("version",
                                           "Show version information and exit.",
                                           true,
                                           t => t.Writer.WriteVersion(t.Parent!));
