namespace Lola.Tasks.Handlers;

public class TaskHandler(ITaskDataSource dataSource, ILogger<TaskHandler> logger) : ITaskHandler {
    private readonly ITaskDataSource _dataSource = dataSource;
    private readonly ILogger<TaskHandler> _logger = logger;

    public TaskEntity[] List() => _dataSource.GetAll();

    public TaskEntity? GetByKey(uint key) => _dataSource.FindByKey(key);
    public TaskEntity? GetByName(string name) => _dataSource.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    public TaskEntity Create(Action<TaskEntity> setUp)
        => _dataSource.Create(setUp);

    public void Add(TaskEntity task) {
        if (_dataSource.FindByKey(task.Key) != null)
            throw new InvalidOperationException($"A task with the key '{task.Key}' already exists.");

        _dataSource.Add(task);
        _logger.LogInformation("Added new task: {TaskKey} => {TaskName}", task.Name, task.Key);
    }

    public void Update(TaskEntity task) {
        if (_dataSource.FindByKey(task.Key) == null)
            throw new InvalidOperationException($"Task with key '{task.Key}' not found.");

        _dataSource.Update(task);
        _logger.LogInformation("Updated task: {TaskKey} => {TaskName}", task.Name, task.Key);
    }

    public void Remove(uint key) {
        var task = _dataSource.FindByKey(key)
                     ?? throw new InvalidOperationException($"Task with key '{key}' not found.");

        _dataSource.Remove(key);
        _logger.LogInformation("Removed task: {TaskKey} => {TaskName}", task.Name, task.Key);
    }
}
