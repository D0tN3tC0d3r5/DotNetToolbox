namespace DotNetToolbox.AI.Jobs;

public abstract class AnalysisJob : Job<string, string> {
    protected AnalysisJob(string id, IAgentFactory agentFactory) : base(id, agentFactory) {
        Type = JobType.Analysis;
    }

    protected AnalysisJob(IGuidProvider guid, IAgentFactory agentFactory) : base(guid, agentFactory) {
        Type = JobType.Analysis;
    }
}
