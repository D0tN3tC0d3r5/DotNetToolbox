namespace Sophia.WebApp.Services;

public class WorldService(ApplicationDbContext dbContext)
    : IWorldService {
    public async Task<WorldData> GetWorld() {
        try {
            var world = await dbContext.Worlds
                                       .AsNoTracking()
                                       .FirstAsync();
            return world.ToDto();
        } catch (Exception ex) {
            Console.WriteLine(ex);
            throw;
        }
    }

    public async Task UpdateWorld(WorldData world) {
        var entity = await dbContext.Worlds.FirstAsync();
        entity.UpdateFrom(world);
        await dbContext.SaveChangesAsync();
    }
}
