namespace DotNetToolbox.OpenAI.Commands;

internal interface ICommand
{
    public string Name { get; }
    public string Description { get; }
    public string[] Aliases { get; }

    public void Execute(string[] parameters);
}
