namespace Sophia.WebApp.Services;

public class WorldService(ApplicationDbContext dbContext)
    : IWorldService {
    public async Task<WorldData> GetWorld() {
        try {
            var world = await dbContext.Worlds
                                       .Include(w => w.Facts)
                                       .Include(w => w.Tools)
                                       .AsNoTracking()
                                       .FirstAsync();
            return world.ToDto();
        } catch (Exception ex) {
            Console.WriteLine(ex);
            throw;
        }
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
