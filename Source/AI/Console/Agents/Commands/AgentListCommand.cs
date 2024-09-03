namespace AI.Sample.Agents.Commands;

public class AgentListCommand(IHasChildren parent, IAgentHandler agentHandler)
    : Command<AgentListCommand>(parent, "List", ["ls"]) {
    private readonly IAgentHandler _agentHandler = agentHandler;

    protected override Task<Result> Execute(CancellationToken ct = default) {
        var agents = _agentHandler.List();

        var table = new Table();
        table.AddColumn("Name");
        table.AddColumn("Is IsSelected");

        foreach (var agent in agents) {
            table.AddRow(
                agent.Name,
                agent.Key == _agentHandler.Selected?.Key ? "[green]Yes[/]" : "No"
            );
        }

        Output.Write(table);
        return Result.SuccessTask();
    }
}
