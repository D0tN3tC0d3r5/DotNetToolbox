namespace DotNetToolbox.AI.Jobs;

public abstract class ChatJob : Job<string[], string> {
    protected ChatJob(string id, IAgentFactory agentFactory) : base(id, agentFactory) {
        Type = JobType.Chat;
    }

    protected ChatJob(IGuidProvider guid, IAgentFactory agentFactory) : base(guid, agentFactory) {
        Type = JobType.Chat;
    }
}
