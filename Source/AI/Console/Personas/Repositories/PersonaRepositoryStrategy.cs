namespace AI.Sample.Personas.Repositories;

public class PersonaRepositoryStrategy(IConfigurationRoot configuration)
    : JsonFilePerTypeRepositoryStrategy<IPersonaRepository, PersonaEntity, uint>("personas", configuration),
      IPersonaRepositoryStrategy {
    protected override uint FirstKey { get; } = 1;

    protected override bool TryGenerateNextKey(out uint next) {
        next = LastUsedKey == default ? FirstKey : ++LastUsedKey;
        return true;
    }
}
