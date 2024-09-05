namespace AI.Sample.Tasks.Handlers;

public interface ITaskHandler {
    TaskEntity[] List();
    TaskEntity? GetByKey(uint key);
    TaskEntity? GetByName(string name);
    TaskEntity Create(Action<TaskEntity> setUp);
    void Add(TaskEntity task);
    void Update(TaskEntity task);
    void Remove(uint key);
}
