namespace Sophia.Services;

public class WorldService(DataContext dbContext)
    : IWorldService {
    public async Task<WorldData> GetWorld() {
        try {
            var world = await dbContext.Worlds.FindFirst();
            return world!;
        }
        catch (Exception ex) {
            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task UpdateWorld(WorldData input) {
        await dbContext.Worlds.Update(input);
        await dbContext.SaveChanges();
    }
}
