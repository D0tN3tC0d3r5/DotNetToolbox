using DotNetToolbox.Data.DataSources;

namespace Lola.Tasks.Repositories;

public class TaskDataSource(ITaskStorage strategy)
    : DataSource<ITaskStorage, TaskEntity, uint>(strategy),
      ITaskDataSource;
