namespace Sophia.Data.Providers;

public class ModelsDbSet(ApplicationDbContext dbContext) : ModelRepository {
    public override async Task<IReadOnlyList<ModelData>> ToArrayAsync(CancellationToken ct = default)
        => await dbContext.Models
                          .AsNoTracking()
                          .Include(i => i.Provider)
                          .Select(i => Mapper.ToModelData(i, true))
                          .ToArrayAsync(ct);

    public override async Task<ModelData?> FindByKey(string key, CancellationToken ct = default) {
        var entity = await dbContext.Models
                                    .AsNoTracking()
                                    .Include(i => i.Provider)
                                    .FirstOrDefaultAsync(i => i.Id == key, ct);
        return entity == null
            ? null
            : Mapper.ToModelData(entity, includeProvider: true);
    }

    public override async ValueTask Add(ModelData input, CancellationToken ct = default) {
        var alreadyExists = await dbContext.Models.AnyAsync(i => i.Id == input.Id, ct);
        if (alreadyExists)
            return;
        var entity = Mapper.ToModelEntity(input);
        await dbContext.Models.AddAsync(entity, ct);
        await dbContext.SaveChangesAsync(ct);
    }

    public override async Task Update(ModelData input, CancellationToken ct = default) {
        var entity = await dbContext.Models
                                    .Include(i => i.Provider)
                                    .FirstOrDefaultAsync(i => i.Id == input.Id, ct);
        if (entity == null)
            return;
        Mapper.UpdateModelEntity(input, entity);
        await dbContext.SaveChangesAsync(ct);
    }

    public override async Task Remove(string key, CancellationToken ct = default) {
        var entity = await dbContext.Models
                                    .FirstOrDefaultAsync(i => i.Id == key, ct);
        if (entity == null)
            return;
        dbContext.Models.Remove(entity);
        await dbContext.SaveChangesAsync(ct);
    }
}
