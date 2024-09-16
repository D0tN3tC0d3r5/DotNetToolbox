using DotNetToolbox.Data.DataSources;

namespace Lola.Tasks.Repositories;

public interface ITaskDataSource
    : IDataSource<TaskEntity, uint>;
