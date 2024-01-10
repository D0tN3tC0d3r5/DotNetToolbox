namespace DotNetToolbox.OpenAI.Commands;

public class ClearScreenCommand : ICommand
{
    public string Name => "ClearScreen";
    public string Description => "Clears the terminal screen.";
    public string[] Aliases => ["cls"];

    public void Execute(string[] parameters)
        => Console.Clear();
}
