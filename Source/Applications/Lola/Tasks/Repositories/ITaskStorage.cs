using DotNetToolbox.Data.Storages;

namespace Lola.Tasks.Repositories;

public interface ITaskStorage
    : IStorage<TaskEntity, uint>;
