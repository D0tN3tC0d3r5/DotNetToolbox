namespace DotNetToolbox.CommandLineBuilder.Defaults;

internal sealed class NoColorFlag() : Flag("no-color",
                                           "Don't colorize output.",
                                           false,
                                           t => t.Writer.UseColors = false);
