namespace DotNetToolbox.AI.Jobs;

public abstract class GenerationJob : Job<string, byte[]> {
    protected GenerationJob(string id, IAgentFactory agentFactory) : base(id, agentFactory) {
        Type = JobType.Generation;
    }

    protected GenerationJob(IGuidProvider guid, IAgentFactory agentFactory) : base(guid, agentFactory) {
        Type = JobType.Generation;
    }
}
