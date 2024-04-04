
namespace Sophia.Data.World;

public class WorldDbSet(ApplicationDbContext dbContext) : WorldRepository {
    public override Task<bool> HaveAny(CancellationToken ct = default)
        => dbContext.Worlds.AnyAsync(ct);

    public override async Task<WorldData?> FindFirst(CancellationToken ct = default) {
        var entity = await dbContext.Worlds.FirstAsync(ct);
        return Mapper.ToWorldData(entity);
    }

    public override async ValueTask Add(WorldData input, CancellationToken ct = default) {
        var entity = Mapper.ToWorldEntity(input);
        await dbContext.Worlds.AddAsync(entity, ct);
        await dbContext.SaveChangesAsync(ct);
    }

    public override async Task Update(WorldData input, CancellationToken ct = default) {
        var entity = await dbContext.Worlds.FindAsync(input.Id, ct);
        if (entity == null)
            return;
        Mapper.UpdateWorldEntity(input, entity);
        await dbContext.SaveChangesAsync(ct);
    }
}
