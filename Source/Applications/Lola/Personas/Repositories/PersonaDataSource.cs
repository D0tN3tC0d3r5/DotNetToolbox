namespace Lola.Personas.Repositories;

public class PersonaDataSource(IPersonaStorage storage)
    : DataSource<IPersonaStorage, PersonaEntity, uint>(storage),
      IPersonaDataSource;
