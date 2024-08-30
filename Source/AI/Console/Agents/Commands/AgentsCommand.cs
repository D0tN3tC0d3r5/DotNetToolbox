namespace AI.Sample.Agents.Commands;

public class AgentsCommand
    : Command<AgentsCommand> {
    public AgentsCommand(IHasChildren parent)
        : base(parent, "Agents", []) {
        Description = "Manage AI agents.";

        AddCommand<AgentListCommand>();
        AddCommand<AgentAddCommand>();
        AddCommand<AgentSelectCommand>();
        AddCommand<AgentInfoCommand>();
        AddCommand<HelpCommand>();
    }
}
