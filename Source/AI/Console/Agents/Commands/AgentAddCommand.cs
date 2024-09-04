namespace AI.Sample.Agents.Commands;

public class AgentAddCommand(IHasChildren parent, IAgentHandler handler)
    : Command<AgentAddCommand>(parent, "Create", ["add", "new"]) {
    protected override Task<Result> Execute(CancellationToken ct = default) {
        var agent = handler.Create(p => p.Name = Input.Ask("Enter the agent name:"));
        try {
            handler.Register(agent);
            Output.WriteLine($"[green]Agent '{agent.Name}' added successfully.[/]");
            Output.WriteLine();

            return Result.SuccessTask();
        }
        catch (Exception ex) {
            Output.WriteError("Error adding an agent.");
            Output.WriteLine();

            return Result.ErrorTask(ex.Message);
        }
    }
}
