namespace DotNetToolbox.CommandLineBuilder.TestUtilities;

public class InMemoryOutputWriter : OutputWriter {
    public string Output = string.Empty;

    public override void WriteLine() => Output += "\n";
    public override void WriteLine(string? value) => Output += $"{value}\n";
}
