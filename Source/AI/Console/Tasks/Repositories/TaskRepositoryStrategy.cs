namespace AI.Sample.Tasks.Repositories;

public class TaskRepositoryStrategy(IConfigurationRoot configuration)
    : JsonFileRepositoryStrategy<ITaskRepository, TaskEntity, uint>("tasks", configuration),
      ITaskRepositoryStrategy {
    protected override uint FirstKey { get; } = 1;

    protected override bool TryGenerateNextKey(out uint next) {
        next = LastUsedKey == default ? FirstKey : ++LastUsedKey;
        return true;
    }
}
