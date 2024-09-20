namespace Lola.Tasks.Handlers;

public class TaskHandler(ITaskDataSource dataSource, ILogger<TaskHandler> logger) : ITaskHandler {
    public TaskEntity[] List() => dataSource.GetAll();

    public TaskEntity? GetById(uint id) => dataSource.FindByKey(id);
    public TaskEntity? Find(Expression<Func<TaskEntity, bool>> predicate) => dataSource.Find(predicate);

    public TaskEntity Create(Action<TaskEntity> setUp)
        => dataSource.Create(setUp);

    public void Add(TaskEntity task) {
        if (dataSource.FindByKey(task.Id) != null)
            throw new InvalidOperationException($"A task with the id '{task.Id}' already exists.");

        dataSource.Add(task);
        logger.LogInformation("Added new task: {TaskId} => {TaskName}", task.Name, task.Id);
    }

    public void Update(TaskEntity task) {
        if (dataSource.FindByKey(task.Id) == null)
            throw new InvalidOperationException($"Task with id '{task.Id}' not found.");

        dataSource.Update(task);
        logger.LogInformation("Updated task: {TaskId} => {TaskName}", task.Name, task.Id);
    }

    public void Remove(uint id) {
        var task = dataSource.FindByKey(id)
                     ?? throw new InvalidOperationException($"Task with id '{id}' not found.");

        dataSource.Remove(id);
        logger.LogInformation("Removed task: {TaskId} => {TaskName}", task.Name, task.Id);
    }
}
