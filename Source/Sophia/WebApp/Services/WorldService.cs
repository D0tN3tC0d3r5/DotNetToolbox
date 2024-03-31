namespace Sophia.WebApp.Services;

public class WorldService(DataContext dbContext)
    : IWorldService {
    public async Task<WorldData> GetWorld() {
        try {
            var world = await dbContext.Worlds
                                       .AsNoTracking()
                                       .FirstAsync();
            return world;
        }
        catch (Exception ex) {
            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task UpdateWorld(WorldData world) {
        await dbContext.Worlds.Update(world);
        await dbContext.SaveChanges();
    }
}
