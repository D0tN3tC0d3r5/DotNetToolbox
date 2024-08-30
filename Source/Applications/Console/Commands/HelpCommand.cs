﻿namespace DotNetToolbox.ConsoleApplication.Commands;

internal sealed class HelpCommand
    : Command<HelpCommand> {
    private readonly IHasChildren _parent;

    public HelpCommand(IHasChildren parent)
        : base(parent, "Help", ["?"]) {
        _parent = IsNotNull(parent);
        Description = "Display this help information.";
        AddParameter("Target", string.Empty);
    }

    protected override Task<Result> Execute(CancellationToken ct = default) {
        var target = (string?)Context.GetValueOrDefault("Target");
        var command = _parent.Commands.FirstOrDefault(i => i.Name.Equals(target, StringComparison.OrdinalIgnoreCase));
        var node = command ?? _parent;
        Output.WriteLine(node.ToHelp());
        return SuccessTask();
    }
}