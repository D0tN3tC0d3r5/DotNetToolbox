namespace Lola.Personas.Repositories;

public class PersonaStorage(IConfiguration configuration)
    : JsonFilePerTypeStorage<PersonaEntity, uint>("personas", configuration),
      IPersonaStorage {
    protected override uint FirstKey { get; } = 1;

    protected override bool TryGenerateNextKey(out uint next) {
        next = LastUsedKey == default ? FirstKey : ++LastUsedKey;
        return true;
    }
}
