namespace DotNetToolbox.OpenAI.Commands;

public class ExitCommand : ICommand
{
    public string Name => "Exit";
    public string Description => "Exits the application.";
    public string[] Aliases => ["x"];

    public void Execute(string[] parameters)
        => Environment.Exit(0);
}
