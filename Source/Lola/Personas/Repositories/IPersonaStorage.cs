using DotNetToolbox.Data.Storages;

namespace Lola.Personas.Repositories;

public interface IPersonaStorage
    : IStorage<PersonaEntity, uint>;
