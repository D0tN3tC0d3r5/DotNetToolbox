namespace AI.Sample.Agents.Handlers;

public interface IAgentHandler {
    AgentEntity? Selected { get; }

    AgentEntity Create(Action<AgentEntity> setUp);
    void Register(AgentEntity model);
    AgentEntity[] List();
    AgentEntity? GetByKey(uint key);
    void Update(AgentEntity model);
    void Remove(uint key);

    void Select(uint key);
}
