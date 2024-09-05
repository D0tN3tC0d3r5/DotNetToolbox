namespace AI.Sample.Tasks.Repositories;

public class TaskRepository(ITaskRepositoryStrategy strategy)
    : Repository<ITaskRepositoryStrategy, TaskEntity, uint>(strategy),
      ITaskRepository;
