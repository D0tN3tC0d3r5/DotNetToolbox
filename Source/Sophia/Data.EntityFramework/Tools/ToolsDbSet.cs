namespace Sophia.Data.Tools;

public class ToolsDbSet(ApplicationDbContext dbContext) : ToolRepository {
    public override async Task<IReadOnlyList<ToolData>> ToArrayAsync(CancellationToken ct = default)
        => await dbContext.Tools
                          .AsNoTracking()
                          .Select(i => Mapper.ToToolData(i))
                          .ToArrayAsync(ct);

    public override async Task<ToolData?> FindByKey(int key, CancellationToken ct = default) {
        var entity = await dbContext.Tools
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(i => i.Id == key, ct);
        return entity == null
            ? null
            : Mapper.ToToolData(entity);
    }

    public override async ValueTask Add(ToolData input, CancellationToken ct = default) {
        var alreadyExists = await dbContext.Tools.AnyAsync(i => i.Id == input.Id, ct);
        if (alreadyExists)
            return;
        var entity = Mapper.ToToolEntity(input);
        await dbContext.Tools.AddAsync(entity, ct);
        await dbContext.SaveChangesAsync(ct);
    }

    public override async Task Update(ToolData input, CancellationToken ct = default) {
        var entity = await dbContext.Tools
                                    .FirstOrDefaultAsync(i => i.Id == input.Id, ct);
        if (entity == null)
            return;
        Mapper.UpdateToolEntity(input, entity);
        await dbContext.SaveChangesAsync(ct);
    }

    public override async Task Remove(int key, CancellationToken ct = default) {
        var entity = await dbContext.Tools
                                    .FirstOrDefaultAsync(i => i.Id == key, ct);
        if (entity == null)
            return;
        dbContext.Tools.Remove(entity);
        await dbContext.SaveChangesAsync(ct);
    }
}
