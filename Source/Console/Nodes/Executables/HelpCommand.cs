﻿namespace DotNetToolbox.ConsoleApplication.Nodes.Executables;

internal sealed class HelpCommand : Command<HelpCommand> {
    private readonly IHasChildren _parent;

    public HelpCommand(IHasChildren parent)
        : base(parent, "Help", "?") {
        _parent = parent;
        Description = "Display help information.";
    }

    protected override Result Execute() {
        var builder = new HelpBuilder(_parent, includeApplication: false);
        var help = builder.Build();
        Application.Output.Write(help);
        return Success();
    }
}
