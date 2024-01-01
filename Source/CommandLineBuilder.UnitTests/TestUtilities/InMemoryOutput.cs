namespace DotNetToolbox.CommandLineBuilder.TestUtilities;

public class InMemoryOutput : OutputWriter {
    public StringBuilder Output = new();

    public override void WriteLine() => Output.AppendLine();
    public override void WriteLine(string? value) => Output.AppendLine(value);
}
