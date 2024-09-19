namespace Lola.Tasks.Handlers;

public class TaskHandler(ITaskDataSource dataSource, ILogger<TaskHandler> logger) : ITaskHandler {
    private readonly ITaskDataSource _dataSource = dataSource;
    private readonly ILogger<TaskHandler> _logger = logger;

    public TaskEntity[] List() => _dataSource.GetAll();

    public TaskEntity? GetById(uint id) => _dataSource.FindByKey(id);
    public TaskEntity? GetByName(string name) => _dataSource.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    public TaskEntity Create(Action<TaskEntity> setUp)
        => _dataSource.Create(setUp);

    public void Add(TaskEntity task) {
        if (_dataSource.FindByKey(task.Id) != null)
            throw new InvalidOperationException($"A task with the id '{task.Id}' already exists.");

        _dataSource.Add(task);
        _logger.LogInformation("Added new task: {TaskId} => {TaskName}", task.Name, task.Id);
    }

    public void Update(TaskEntity task) {
        if (_dataSource.FindByKey(task.Id) == null)
            throw new InvalidOperationException($"Task with id '{task.Id}' not found.");

        _dataSource.Update(task);
        _logger.LogInformation("Updated task: {TaskId} => {TaskName}", task.Name, task.Id);
    }

    public void Remove(uint id) {
        var task = _dataSource.FindByKey(id)
                     ?? throw new InvalidOperationException($"Task with id '{id}' not found.");

        _dataSource.Remove(id);
        _logger.LogInformation("Removed task: {TaskId} => {TaskName}", task.Name, task.Id);
    }
}
