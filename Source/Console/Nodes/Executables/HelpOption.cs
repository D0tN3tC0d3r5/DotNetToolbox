namespace DotNetToolbox.ConsoleApplication.Nodes.Executables;

internal sealed class HelpOption
    : Command<HelpOption> {
    public HelpOption(IHasChildren parent)
        : base(parent, "--help", "-h", "-?") {
        Description = "Display help information.";
    }

    protected override Result Execute() {
        var help = HelpBuilder.Build(Parent, includeApplication: true); ;
        Application.Output.Write(help);
        return Success();
    }
}
