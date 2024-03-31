namespace Sophia.WebApp.Services;

public class PersonasService(DataContext dbContext)
    : IPersonasService {
    public async Task<IReadOnlyList<PersonaData>> GetList(string? filter = null) {
        try {
            return await dbContext.Personas
                                  .AsNoTracking()
                                  .Include(p => p.KnownTools)
                                  .ToArrayAsync();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<PersonaData?> GetById(int id) {
        var entity = await dbContext.Personas
                                    .Include(p => p.KnownTools)
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(s => s.Id == id);
        return entity;
    }

    public async Task Add(PersonaData persona) {
        await dbContext.Personas.Add(persona);
        await dbContext.SaveChanges();
    }

    public async Task Update(PersonaData persona) {
        if (await dbContext.Personas.AllAsync(s => s.Id != persona.Id)) return;
        await dbContext.Personas.Update(persona);
        await dbContext.SaveChanges();
    }

    public async Task Delete(int id) {
        var entity = await dbContext.Personas.FirstOrDefaultAsync(s => s.Id == id);
        if (entity is null) return;
        await dbContext.Personas.Remove(entity);
        await dbContext.SaveChanges();
    }
}
