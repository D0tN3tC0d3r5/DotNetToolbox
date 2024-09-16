namespace Lola.Tasks.Repositories;

public class TaskStorage(IConfiguration configuration)
    : JsonFilePerTypeStorage<TaskEntity, uint>("tasks", configuration),
      ITaskStorage {
    protected override uint FirstKey { get; } = 1;

    protected override bool TryGenerateNextKey(out uint next) {
        next = LastUsedKey == default ? FirstKey : ++LastUsedKey;
        return true;
    }
}
