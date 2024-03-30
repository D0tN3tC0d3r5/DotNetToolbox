namespace Sophia.WebApp.Services;

public class PersonasService(ApplicationDbContext dbContext)
    : IPersonasService {
    public async Task<IReadOnlyList<PersonaData>> GetList(string? filter = null) {
        try {
            return await dbContext.Personas
                                  .AsNoTracking()
                                  .Include(p => p.Tools)
                                  .Select(p => p.ToDto())
                                  .ToArrayAsync();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<PersonaData?> GetById(int id) {
        var entity = await dbContext.Personas
                                    .Include(p => p.Tools)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(s => s.Id == id);
        return entity?.ToDto();
    }

    public async Task Add(PersonaData persona) {
        var entity = persona.ToEntity();
        dbContext.Personas.Add(entity);
        await dbContext.SaveChangesAsync();
        persona.Id = entity.Id;
    }

    public async Task Update(PersonaData persona) {
        var entity = await dbContext.Personas
                                    .FirstOrDefaultAsync(s => s.Id == persona.Id);
        entity?.UpdateFrom(persona);
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
