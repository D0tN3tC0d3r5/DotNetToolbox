namespace AI.Sample.Personas.Repositories;

public class PersonaRepository(IPersonaRepositoryStrategy strategy)
    : Repository<IPersonaRepositoryStrategy, PersonaEntity, uint>(strategy),
      IPersonaRepository;
