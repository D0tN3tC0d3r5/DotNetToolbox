namespace AI.Sample.Agents.Repositories;

public class AgentRepositoryStrategy(IConfigurationRoot configuration,
                                     Lazy<IAgentRepository> repository)
    : JsonFileRepositoryStrategy<IAgentRepository, AgentEntity, uint, INumericSequencer>("agents", configuration, repository),
      IAgentRepositoryStrategy {
    protected override uint FirstKey { get; } = 1;

    protected override Result<uint> GenerateNextKey() {
        if (LastUsedKey == default) LastUsedKey = FirstKey;
        else ++LastUsedKey;
        return Result.Success(LastUsedKey);
    }
}
