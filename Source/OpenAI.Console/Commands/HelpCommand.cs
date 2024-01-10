namespace DotNetToolbox.OpenAI.Commands;

public class HelpCommand : ICommand
{
    public string Name => "Help";
    public string Description => "Displays available commands.";
    public string[] Aliases => ["h", "?"];

    public void Execute(string[] parameters)
    {
        Console.WriteLine();
        Console.WriteLine("Available commands:");
        foreach (var command in CommandRegistry.Commands.Values)
        {
            var aliases = command.Aliases.Length == 0 ? "" : $" ({string.Join(", ", command.Aliases)})";
            Console.WriteLine($"{command.Name}{aliases} - {command.Description}");
        }
        Console.WriteLine();
    }
}
