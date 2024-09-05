namespace AI.Sample.Tasks.Handlers;

public class TaskHandler(ITaskRepository repository, ILogger<TaskHandler> logger) : ITaskHandler {
    private readonly ITaskRepository _repository = repository;
    private readonly ILogger<TaskHandler> _logger = logger;

    public TaskEntity[] List() => _repository.GetAll();

    public TaskEntity? GetByKey(uint key) => _repository.FindByKey(key);
    public TaskEntity? GetByName(string name) => _repository.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    public TaskEntity Create(Action<TaskEntity> setUp)
        => _repository.Create(setUp);

    public void Add(TaskEntity task) {
        if (_repository.FindByKey(task.Key) != null)
            throw new InvalidOperationException($"A task with the key '{task.Key}' already exists.");

        _repository.Add(task);
        _logger.LogInformation("Added new task: {TaskKey} => {TaskName}", task.Name, task.Key);
    }

    public void Update(TaskEntity task) {
        if (_repository.FindByKey(task.Key) == null)
            throw new InvalidOperationException($"Task with key '{task.Key}' not found.");

        _repository.Update(task);
        _logger.LogInformation("Updated task: {TaskKey} => {TaskName}", task.Name, task.Key);
    }

    public void Remove(uint key) {
        var task = _repository.FindByKey(key)
                     ?? throw new InvalidOperationException($"Task with key '{key}' not found.");

        _repository.Remove(key);
        _logger.LogInformation("Removed task: {TaskKey} => {TaskName}", task.Name, task.Key);
    }
}
