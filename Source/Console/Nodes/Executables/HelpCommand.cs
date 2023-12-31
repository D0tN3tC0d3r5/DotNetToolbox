﻿namespace DotNetToolbox.ConsoleApplication.Nodes.Executables;

internal sealed class HelpCommand : Command<HelpCommand> {
    private readonly IHasChildren _parent;

    public HelpCommand(IHasChildren parent)
        : base(parent, "Help", "?") {
        _parent = parent;
        Description = "Display help information.";
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct) {
        var builder = new HelpBuilder(_parent, includeApplication: false);
        Application.Output.Write(builder.Build());
        return SuccessTask();
    }
}
