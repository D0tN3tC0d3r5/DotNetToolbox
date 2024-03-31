namespace Sophia.Data.Personas;

public class PersonasRepository(DataContext dataContext, DbContext dbContext)
    : EntityFrameworkRepository<PersonaData, int, PersonaEntity, int>(dataContext, dbContext) {
    //protected override Expression<Func<PersonaEntity, PersonaData>> Project { get; }
    //    = input => Mapper.ToPersonaData(input);

    //protected override Action<PersonaData, PersonaEntity> UpdateFrom { get; }
    //    = Mapper.UpdatePersonaEntity;
}
