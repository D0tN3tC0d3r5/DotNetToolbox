namespace Lola.Tasks.Repositories;

public class TaskDataSource(ITaskStorage storage)
    : DataSource<ITaskStorage, TaskEntity, uint>(storage),
      ITaskDataSource;
