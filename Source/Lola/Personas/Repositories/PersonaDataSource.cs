using DotNetToolbox.Data.DataSources;

namespace Lola.Personas.Repositories;

public class PersonaDataSource(IPersonaStorage strategy)
    : DataSource<IPersonaStorage, PersonaEntity, uint>(strategy),
      IPersonaDataSource;
