namespace AI.Sample.Agents.Handlers;

public class AgentHandler(IApplication application, IAgentRepository repository, ILogger<AgentHandler> logger)
    : IAgentHandler {
    private readonly IApplication _application = application;
    private readonly IAgentRepository _repository = repository;
    private readonly ILogger<AgentHandler> _logger = logger;

    private const string CurrentAgentKey = "IsSelected";

    public AgentEntity[] List() => _repository.GetAll();

    public AgentEntity? Selected {
        get => _application.Context.TryGetValue(CurrentAgentKey, out var agent) ? agent as AgentEntity : null;
        private set => _application.Context[CurrentAgentKey] = value!;
    }

    public AgentEntity? GetByKey(uint key) => _repository.FindByKey(key);

    public AgentEntity Create(Action<AgentEntity> setUp)
        => _repository.Create(setUp);

    public void Register(AgentEntity model) {
        if (_repository.FindByKey(model.Key) != null)
            throw new InvalidOperationException($"An model with the name '{model.Key}' already exists.");

        _repository.Add(model);
        _logger.LogInformation("Added new agent: {AgentKey} => {AgentName}", model.Key, model.Name);
    }

    public void Update(AgentEntity model) {
        if (string.IsNullOrWhiteSpace(model.Name))
            throw new ArgumentException("Agent key (name) cannot be empty.", nameof(model));

        _repository.Update(model);
        _logger.LogInformation("Updated agent: {AgentKey} => {AgentName}", model.Key, model.Name);

        if (Selected?.Key == model.Key)
            Selected = model;
    }

    public void Remove(uint key) {
        var agent = _repository.FindByKey(key)
                 ?? throw new InvalidOperationException($"Agent with key '{key}' not found.");

        _repository.Remove(key);
        _logger.LogInformation("Removed agent: {AgentKey} => {AgentName}", agent.Name, agent.Key);

        if (Selected?.Key == key)
            Selected = null;
    }

    public void Select(uint key) {
        Selected = _repository.FindByKey(key)
                ?? throw new InvalidOperationException($"Agent with key '{key}' not found.");
        _logger.LogInformation("IsSelected model: {AgentKey} => {AgentName}", Selected.Key, Selected.Name);
    }
}
