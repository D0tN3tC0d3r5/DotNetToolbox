namespace AI.Sample.Agents.Commands;

public class AgentAddCommand(IHasChildren parent, IAgentHandler handler)
    : Command<AgentAddCommand>(parent, "Add", ["new"]) {
    protected override Task<Result> Execute(CancellationToken ct = default) {
        var agent = handler.Create(p => p.Name = AnsiConsole.Ask<string>("Enter the agent name:"));
        try {
            handler.Register(agent);
            Output.WriteLine($"[green]Agent '{agent.Name}' added successfully.[/]");
            return Result.SuccessTask();
        }
        catch (Exception ex) {
            Output.WriteError(ex, "Error adding an agent.");
            return Result.ErrorTask(ex.Message);
        }
    }
}
