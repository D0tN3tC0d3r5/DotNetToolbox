namespace Sophia.Data.World;

public class WorldRepository(ApplicationDbContext dbContext)
    : EntityFrameworkRepository<WorldData, WorldEntity, Guid>(dbContext.Worlds) {
    protected override Expression<Func<WorldEntity, WorldData>> ProjectTo { get; }
        = input => Mapper.ToWorldData(input);
    protected override Action<WorldData, WorldEntity> UpdateFrom { get; }
        = Mapper.UpdateWorldEntity;
    protected override Func<WorldData, WorldEntity> CreateFrom { get; }
        = Mapper.ToWorldEntity;
}
