namespace AI.Sample.Agents.Repositories;

public class AgentRepositoryStrategy(IConfigurationRoot configuration)
    : JsonFileRepositoryStrategy<IAgentRepository, AgentEntity, uint>("agents", configuration),
      IAgentRepositoryStrategy {
    protected override uint FirstKey { get; } = 1;

    protected override bool TryGenerateNextKey(out uint next) {
        next = LastUsedKey == default ? FirstKey : ++LastUsedKey;
        return true;
    }
}
