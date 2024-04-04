namespace Sophia.Services;

public class PersonasService(DataContext dbContext)
    : IPersonasService {
    public async Task<IReadOnlyList<PersonaData>> GetList(string? filter = null) {
        try {
            var list = await dbContext.Personas.ToArrayAsync();
            return list;
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task<PersonaData?> GetById(int id) {
        try {
            var entity = await dbContext.Personas.FindByKey(id);
            return entity;
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task Add(PersonaData input) {
        try {
            await dbContext.Personas.Add(input);
            await dbContext.SaveChanges();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task Update(PersonaData input) {
        try {
            await dbContext.Personas.Update(input);
            await dbContext.SaveChanges();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }

    public async Task Delete(int id) {
        try {
            await dbContext.Personas.Remove(id);
            await dbContext.SaveChanges();
        }
        catch (Exception ex) {
            Console.WriteLine(ex.Message);
            throw;
        }
    }
}
