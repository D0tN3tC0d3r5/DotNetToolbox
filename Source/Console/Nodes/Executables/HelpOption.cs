namespace DotNetToolbox.ConsoleApplication.Nodes.Executables;

internal sealed class HelpOption
    : Command<HelpOption> {
    public HelpOption(IHasChildren parent)
        : base(parent, "--help", "-h", "-?") {
        Description = "Display help information.";
    }

    protected override Result Execute() {
        var builder = new HelpBuilder(Parent, includeApplication: true);
        Application.Output.Write(builder.Build());
        return Success();
    }
}
