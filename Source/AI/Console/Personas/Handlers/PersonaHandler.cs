namespace AI.Sample.Personas.Handlers;

public class PersonaHandler(IPersonaRepository repository, ILogger<PersonaHandler> logger) : IPersonaHandler {
    private readonly IPersonaRepository _repository = repository;
    private readonly ILogger<PersonaHandler> _logger = logger;

    public PersonaEntity[] List() => _repository.GetAll();

    public PersonaEntity? GetByKey(uint key) => _repository.FindByKey(key);
    public PersonaEntity? GetByName(string name) => _repository.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    public PersonaEntity Create(Action<PersonaEntity> setUp)
        => _repository.Create(setUp);

    public void Add(PersonaEntity persona) {
        if (_repository.FindByKey(persona.Key) != null)
            throw new InvalidOperationException($"A persona with the key '{persona.Key}' already exists.");

        _repository.Add(persona);
        _logger.LogInformation("Added new persona: {PersonaKey} => {PersonaName}", persona.Name, persona.Key);
    }

    public void Update(PersonaEntity persona) {
        if (_repository.FindByKey(persona.Key) == null)
            throw new InvalidOperationException($"Persona with key '{persona.Key}' not found.");

        _repository.Update(persona);
        _logger.LogInformation("Updated persona: {PersonaKey} => {PersonaName}", persona.Name, persona.Key);
    }

    public void Remove(uint key) {
        var persona = _repository.FindByKey(key)
                     ?? throw new InvalidOperationException($"Persona with key '{key}' not found.");

        _repository.Remove(key);
        _logger.LogInformation("Removed persona: {PersonaKey} => {PersonaName}", persona.Name, persona.Key);
    }
}
