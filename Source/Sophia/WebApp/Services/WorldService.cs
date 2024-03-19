namespace Sophia.WebApp.Services;

public class WorldService(ApplicationDbContext dbContext, World world)
    : IWorldService {
    public async Task<WorldData> GetWorld() {
        if (await dbContext.Worlds.AnyAsync())
            return world.ToDto();

        var result = world.ToDto();
        dbContext.Worlds.Add(result.ToEntity());
        await dbContext.SaveChangesAsync();
        return result;
    }

    public async Task UpdateWorld(WorldData input) {
        var newWorld = input.ToModel(world.DateTimeProvider);
        var result = newWorld.Validate();
        if (!result.IsSuccess) return;

        world.UpdateFrom(input);

        var entity = input.ToEntity();
        if (!await dbContext.Worlds.AnyAsync()) dbContext.Worlds.Add(entity);
        else dbContext.Worlds.Update(entity);
        await dbContext.SaveChangesAsync();
    }
}
