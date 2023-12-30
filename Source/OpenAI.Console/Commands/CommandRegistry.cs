namespace DotNetToolbox.OpenAI.Commands;

internal static class CommandRegistry
{
    public static Dictionary<string, ICommand> Commands { get; } = [];
    public static Dictionary<string, ICommand> Aliases { get; } = [];

    public static void RegisterCommand(ICommand command)
    {
        Commands[command.Name.ToLowerInvariant()] = command;
        foreach (var alias in command.Aliases)
            Aliases[alias] = command;
    }

    public static ICommand? FindCommand(string commandOrAlias)
        => Commands.GetValueOrDefault(commandOrAlias.ToLowerInvariant())
           ?? Aliases.GetValueOrDefault(commandOrAlias);
}
