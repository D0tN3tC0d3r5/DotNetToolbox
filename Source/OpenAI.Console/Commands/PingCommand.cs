namespace DotNetToolbox.OpenAI.Commands;

internal class PingCommand : Command<PingCommand> {
    public PingCommand()
        : base("Ping", "Send a ping to the watcher process.") {
        SetAction(Ping);
    }

    private void Ping() => Writer.WriteLine($"Ping!");
}
