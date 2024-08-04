namespace DotNetToolbox.AI.Jobs;

public abstract class ResearchJob : Job<string, string[]> {
    protected ResearchJob(string id, IAgentFactory agentFactory) : base(id, agentFactory) {
        Type = JobType.Research;
    }

    protected ResearchJob(IGuidProvider guid, IAgentFactory agentFactory) : base(guid, agentFactory) {
        Type = JobType.Research;
    }
}
