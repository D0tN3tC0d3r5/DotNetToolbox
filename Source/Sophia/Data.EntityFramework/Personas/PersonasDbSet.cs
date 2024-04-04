namespace Sophia.Data.Personas;

public class PersonasDbSet(ApplicationDbContext dbContext) : PersonaRepository {
    public override Task<bool> HaveAny(Expression<Func<PersonaData, bool>> predicate, CancellationToken ct = default) {
        var translator = new LambdaExpressionConversionVisitor<PersonaData, PersonaEntity>();
        var newExpression = translator.Translate<Expression<Func<PersonaEntity, bool>>>(predicate);
        return dbContext.Personas.AnyAsync(newExpression, ct);
    }

    public override async Task<IReadOnlyList<PersonaData>> ToArrayAsync(CancellationToken ct = default)
        => await dbContext.Personas
                          .AsNoTracking()
                          .Include(i => i.Tools)
                          .Select(i => Mapper.ToPersonaData(i))
                          .ToArrayAsync(ct);

    public override async Task<PersonaData?> FindByKey(int key, CancellationToken ct = default) {
        var entity = await dbContext.Personas
                                    .AsNoTracking()
                                    .Include(i => i.Tools)
                                    .FirstOrDefaultAsync(i => i.Id == key, ct);
        return entity == null
            ? null
            : Mapper.ToPersonaData(entity);
    }

    public override async ValueTask Add(PersonaData input, CancellationToken ct = default) {
        var alreadyExists = await dbContext.Personas.AnyAsync(i => i.Id == input.Id, ct);
        if (alreadyExists)
            return;
        var entity = Mapper.ToPersonaEntity(input);
        await dbContext.Personas.AddAsync(entity, ct);
        await dbContext.SaveChangesAsync(ct);
    }

    public override async Task Update(PersonaData input, CancellationToken ct = default) {
        var entity = await dbContext.Personas
                                    .Include(i => i.Tools)
                                    .FirstOrDefaultAsync(i => i.Id == input.Id, ct);
        if (entity == null)
            return;
        Mapper.UpdatePersonaEntity(input, entity);
        await dbContext.SaveChangesAsync(ct);
    }

    public override async Task Remove(int key, CancellationToken ct = default) {
        var entity = await dbContext.Personas
                                    .FirstOrDefaultAsync(i => i.Id == key, ct);
        if (entity == null)
            return;
        dbContext.Personas.Remove(entity);
        await dbContext.SaveChangesAsync(ct);
    }
}
