namespace DotNetToolbox.CommandLineBuilder;

public class RootCommandTests {
    [Fact]
    public void RootCommand_Execute_WithWriter_ExecutesDelegate() {
        InMemoryOutput writer = new();
        RootCommand subject = new(writer);
        subject.SetAction(cmd => {
            var who = cmd.GetValueOrDefault<string>("who");
            cmd.Writer.WriteLine($"Hello {who}!");
        });
        subject.Add(new Parameter<string>("who"));

        subject.Execute("world");

        writer.Output.ToString().Should().Be("Hello world!\r\n");
    }

    [Fact]
    public void RootCommand_Execute_WithoutWriter_ExecutesDelegate() {
        RootCommand subject = new();
        subject.SetAction(cmd => {
            var who = cmd.GetValueOrDefault<string>("who");
            cmd.Writer.WriteLine($"Hello {who}!");
        });
        subject.Add(new Parameter<string>("who"));

        subject.Execute("world");
    }
}
