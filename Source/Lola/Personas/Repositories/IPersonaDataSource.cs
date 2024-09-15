using DotNetToolbox.Data.DataSources;

namespace Lola.Personas.Repositories;

public interface IPersonaDataSource
    : IDataSource<PersonaEntity, uint>;
