namespace AI.Sample.Agents.Commands;

public class AgentSelectCommand(IHasChildren parent, IAgentHandler agentHandler)
    : Command<AgentSelectCommand>(parent, "SelectionPrompt", ["sel"]) {
    private readonly IAgentHandler _agentHandler = agentHandler;

    protected override Task<Result> Execute(CancellationToken ct = default) {
        var agents = _agentHandler.List();

        if (agents.Length == 0) {
            Output.WriteLine("[yellow]No agents available. Please add an agent first.[/]");
            return Result.SuccessTask();
        }

        var selected = Input.SelectionPrompt<AgentEntity>("Select an agent:")
                            .AddChoices(agents)
                            .ConvertWith(c => $"{c.Key}: {c.Name}")
                            .Show();

        try {
            _agentHandler.Select(selected.Key);
            Output.WriteLine($"[green]Agent '{selected.Key}' selected successfully.[/]");
            return Result.SuccessTask();
        }
        catch (Exception ex) {
            Output.WriteError("Error selecting an agent.");
            return Result.ErrorTask(ex.Message);
        }
    }
}
