namespace DotNetToolbox.CommandLineBuilder;

public class RootCommandTests {
    [Fact]
    public async Task RootCommand_Execute_WithWriter_ExecutesDelegate() {
        InMemoryOutputWriter writer = new();
        RootCommand subject = new(writer);
        subject.OnExecute += (cmd, ct) => Task.Run(() => {
            var who = cmd.GetValueOrDefault<string>("who");
            cmd.Writer.WriteLine($"Hello {who}!");
        }, ct);
        subject.Add(new Parameter<string>("who"));

        await subject.Execute("world");

        writer.Output.Should().Be("Hello world!\n");
    }

    [Fact]
    public Task RootCommand_Execute_WithoutWriter_ExecutesDelegate() {
        RootCommand subject = new();
        subject.OnExecute += (cmd, ct) => Task.Run(() => {
            var who = cmd.GetValueOrDefault<string>("who");
            cmd.Writer.WriteLine($"Hello {who}!");
        }, ct);
        subject.Add(new Parameter<string>("who"));

        return subject.Execute("world");
    }
}
