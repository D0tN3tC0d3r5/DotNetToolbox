namespace Sophia.Data.Personas;

public class PersonasRepository(DataContext dataContext, ApplicationDbContext dbContext)
    : EntityFrameworkRepository<PersonaData, PersonaEntity, int>(dataContext, dbContext.Personas) {
    protected override Expression<Func<PersonaEntity, PersonaData>> ProjectTo { get; }
        = input => Mapper.ToPersonaData(input);
    protected override Action<PersonaData, PersonaEntity> UpdateFrom { get; }
        = Mapper.UpdatePersonaEntity;
    protected override Func<PersonaData, PersonaEntity> CreateFrom { get; }
        = Mapper.ToPersonaEntity;
}
