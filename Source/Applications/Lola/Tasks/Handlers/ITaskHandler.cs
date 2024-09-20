namespace Lola.Tasks.Handlers;

public interface ITaskHandler {
    TaskEntity[] List();
    TaskEntity? GetById(uint id);
    TaskEntity? Find(Expression<Func<TaskEntity, bool>> predicate);
    TaskEntity Create(Action<TaskEntity> setUp);
    void Add(TaskEntity task);
    void Update(TaskEntity task);
    void Remove(uint id);
}
