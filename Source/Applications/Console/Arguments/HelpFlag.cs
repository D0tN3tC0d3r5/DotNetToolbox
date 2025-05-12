namespace DotNetToolbox.ConsoleApplication.Arguments;

internal sealed class HelpFlag(IHasChildren parent)
    : Flag<HelpFlag>(parent, "Help", static n => {
        n.Aliases = ["h", "?"];
        n.Description = "Display this help information.";
        n.Help = "Display this help information.";
    }) {
    protected override Task<Result> Execute(CancellationToken ct = default) {
        Output.WriteLine(Parent.ToHelp());
        Application.Exit();
        return Task.FromResult(Success());
    }
}
