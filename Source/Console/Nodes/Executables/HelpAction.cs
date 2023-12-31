﻿namespace DotNetToolbox.ConsoleApplication.Nodes.Executables;

internal sealed class HelpAction : Action<HelpAction> {
    private readonly IHasChildren _parent;

    public HelpAction(IHasChildren parent)
        : base(parent, "--help", "-h") {
        _parent = parent;
        Description = "Display help information.";
    }

    protected override Task<Result> ExecuteAsync(CancellationToken ct) {
        var builder = new HelpBuilder(_parent, includeApplication: true);
        Application.Output.Write(builder.Build());
        return SuccessTask();
    }
}
