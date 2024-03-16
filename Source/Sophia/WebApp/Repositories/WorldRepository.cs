using Sophia.Models.Worlds;
using Sophia.Repositories;
using Sophia.WebApp.Data.World;

namespace Sophia.WebApp.Repositories;

public class WorldRepository(ApplicationDbContext dbContext, World world)
    : IWorldRepository {
    public async Task<WorldData> GetWorld() {
        if (await dbContext.Worlds.AnyAsync())
            return world.ToDto();

        dbContext.Worlds.Add(world.ToEntity());
        await dbContext.SaveChangesAsync();
        return world.ToDto();
    }

    public async Task UpdateWorld(WorldData input) {
        var newWorld = input.ToModel(world.GetProvider());
        var result = newWorld.Validate();
        if (!result.IsSuccess) return;

        world.UpdateFrom(input);

        var entity = input.ToEntity();
        if (!await dbContext.Worlds.AnyAsync()) dbContext.Worlds.Add(entity);
        else dbContext.Worlds.Update(entity);
        await dbContext.SaveChangesAsync();
    }
}
