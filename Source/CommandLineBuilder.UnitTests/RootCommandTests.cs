namespace DotNetToolbox.CommandLineBuilder;

public class RootCommandTests {
    [Fact]
    public void RootCommand_Execute_WithWriter_ExecutesDelegate() {
        InMemoryOutputWriter writer = new();
        RootCommand subject = new(writer);
        subject.SetInstanceAction(cmd => {
            var who = cmd.GetValueOrDefault<string>("who");
            cmd.Writer.WriteLine($"Hello {who}!");
        });
        subject.Add(new Parameter<string>("who"));

        subject.Execute("world");

        writer.Output.Should().Be("Hello world!\n");
    }

    [Fact]
    public void RootCommand_Execute_WithoutWriter_ExecutesDelegate() {
        RootCommand subject = new();
        subject.SetInstanceAction(cmd => {
            var who = cmd.GetValueOrDefault<string>("who");
            cmd.Writer.WriteLine($"Hello {who}!");
        });
        subject.Add(new Parameter<string>("who"));

        subject.Execute("world");
    }
}
