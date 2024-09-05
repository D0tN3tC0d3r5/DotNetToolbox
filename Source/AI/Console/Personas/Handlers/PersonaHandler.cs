namespace AI.Sample.Personas.Handlers;

public class PersonaHandler(IPersonaRepository repository, ILogger<PersonaHandler> logger) : IPersonaHandler {
    public PersonaEntity[] List() => repository.GetAll();

    public PersonaEntity? GetByKey(uint key) => repository.FindByKey(key);
    public PersonaEntity? GetByName(string name) => repository.Find(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

    public PersonaEntity Create(Action<PersonaEntity> setUp)
        => repository.Create(setUp);

    public void Add(PersonaEntity persona) {
        if (repository.FindByKey(persona.Key) != null)
            throw new InvalidOperationException($"A persona with the key '{persona.Key}' already exists.");

        repository.Add(persona);
        logger.LogInformation("Added new persona: {PersonaKey} => {PersonaName}", persona.Name, persona.Key);
    }

    public void Update(PersonaEntity persona) {
        if (repository.FindByKey(persona.Key) == null)
            throw new InvalidOperationException($"Persona with key '{persona.Key}' not found.");

        repository.Update(persona);
        logger.LogInformation("Updated persona: {PersonaKey} => {PersonaName}", persona.Name, persona.Key);
    }

    public void Remove(uint key) {
        var persona = repository.FindByKey(key)
                     ?? throw new InvalidOperationException($"Persona with key '{key}' not found.");

        repository.Remove(key);
        logger.LogInformation("Removed persona: {PersonaKey} => {PersonaName}", persona.Name, persona.Key);
    }
}
