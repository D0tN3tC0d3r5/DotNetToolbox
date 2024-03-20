namespace Sophia.WebApp.Services;

public class PersonasService(ApplicationDbContext dbContext)
    : IPersonasService {
    public async Task<IReadOnlyList<PersonaData>> GetList(string? filter = null) {
        try {
            return await dbContext.Personas
                                  .Include(p => p.Facts)
                                  .Include(p => p.KnownTools)
                                  .AsNoTracking()
                                  .Select(s => s.ToDto())
                                  .ToArrayAsync();
        } catch (Exception ex) {
            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task<PersonaData?> GetById(int id) {
        var entity = await dbContext.Personas.AsNoTracking()
                                    .FirstOrDefaultAsync(s => s.Id == id);
        return entity?.ToDto();
    }

    public async Task Add(PersonaData selectedPersona) {
        var entity = selectedPersona.ToEntity();
        dbContext.Personas.Add(entity);
        await dbContext.SaveChangesAsync();
        selectedPersona.Id = entity.Id;
    }

    public async Task Update(PersonaData input) {
        var entity = await dbContext.Personas
                                    .FirstOrDefaultAsync(s => s.Id == input.Id);
        entity?.UpdateFrom(input);
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(int id) {
        var entity = await dbContext.Personas
                                    .FirstOrDefaultAsync(s => s.Id == id);
        if (entity is null) return;
        dbContext.Personas.Remove(entity);
        await dbContext.SaveChangesAsync();
    }
}
