namespace Sophia.WebApp.Services;

public class WorldService(ApplicationDbContext dbContext)
    : IWorldService {
    public async Task<WorldData> GetWorld() {
        var world = await dbContext.Worlds
                                   .Include(w => w.Facts)
                                   .Include(w => w.UserProfile)
                                   .AsNoTracking()
                                   .FirstAsync();
        return world.ToDto();
    }

    public async Task UpdateWorld(WorldData input) {
        var world = await dbContext.Worlds
                             .Include(w => w.Facts)
                             .Include(w => w.UserProfile)
                             .FirstAsync();
        world.UpdateFrom(input);
        await dbContext.SaveChangesAsync();
    }
}
