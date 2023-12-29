namespace DotNetToolbox.OpenAI.Commands;

internal class MainCommand : RootCommand<MainCommand> {
    public MainCommand() {
        Add(new PingCommand());
    }
}
