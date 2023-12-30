namespace DotNetToolbox.OpenAI.Commands;

public static class CommandParser
{
    public static void ParseAndExecute(string input)
    {
        var segments = input.Split(' ', 2);
        var commandOrAlias = segments[0];
        var parameters = segments.Length > 1 ? segments[1].Split(' ') : [];

        var command = FindCommand(commandOrAlias);
        command?.Execute(parameters);
    }
}
