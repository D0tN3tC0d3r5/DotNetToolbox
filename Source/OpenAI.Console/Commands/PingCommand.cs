namespace DotNetToolbox.OpenAI.Commands;

internal class PingCommand : Command<PingCommand> {
    public PingCommand()
        : base("Ping", "Send a ping to the watcher process.") {
        Command = (_, _, _) => {
                         Ping();
                         return Task.CompletedTask;
                     };
    }

    private void Ping() => Writer.WriteLine($"Ping!");
}
