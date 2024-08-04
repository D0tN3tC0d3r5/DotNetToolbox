namespace DotNetToolbox.AI.Jobs;

public abstract class ActionJob : Job<Tool, bool> {
    protected ActionJob(string id, IAgentFactory agentFactory) : base(id, agentFactory) {
        Type = JobType.Action;
    }

    protected ActionJob(IGuidProvider guid, IAgentFactory agentFactory) : base(guid, agentFactory) {
        Type = JobType.Action;
    }
}
