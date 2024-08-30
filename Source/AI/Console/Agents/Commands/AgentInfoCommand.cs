namespace AI.Sample.Agents.Commands;

public class AgentInfoCommand(IHasChildren parent, IAgentHandler agentHandler)
    : Command<AgentInfoCommand>(parent, "Info", ["i"]) {
    private readonly IAgentHandler _agentHandler = agentHandler;

    protected override Task<Result> Execute(CancellationToken ct = default) {
        var agent = _agentHandler.Selected;

        if (agent == null) {
            Output.WriteLine("[yellow]No agent currently selected.[/]");
            return Result.SuccessTask();
        }

        var table = new Table();
        table.AddColumn("Property");
        table.AddColumn("Value");

        table.AddRow(nameof(AgentEntity.Key), $"{agent.Key}");
        table.AddRow(nameof(AgentEntity.Name), agent.Name);

        Output.Write(table);
        return Result.SuccessTask();
    }
}
