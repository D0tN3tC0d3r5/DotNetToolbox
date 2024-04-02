namespace Sophia.Data.World;

public class WorldDbSet(ApplicationDbContext dbContext) : WorldRepository {
    public override Task<bool> HaveAny(CancellationToken ct = default)
        => dbContext.Worlds.AnyAsync(ct);
}
