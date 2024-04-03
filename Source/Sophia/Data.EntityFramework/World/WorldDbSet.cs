
namespace Sophia.Data.World;

public class WorldDbSet(ApplicationDbContext dbContext) : WorldRepository {
    public override Task<bool> HaveAny(CancellationToken ct = default)
        => dbContext.Worlds.AnyAsync(ct);

    public override async ValueTask Add(WorldData input, CancellationToken ct = default) {
        var entity = Mapper.ToWorldEntity(input);
        await dbContext.Worlds.AddAsync(entity, ct);
        await dbContext.SaveChangesAsync(ct);
    }
}
