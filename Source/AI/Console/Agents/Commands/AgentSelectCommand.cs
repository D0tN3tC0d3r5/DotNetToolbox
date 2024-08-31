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

        var prompt = new SelectionPrompt<AgentEntity>()
                .Title("SelectionPrompt an agent:")
                .AddChoices(agents)
                .UseConverter(c => $"{c.Key}: {c.Name}");
        var selected = AnsiConsole.Prompt(prompt);

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
