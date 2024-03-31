namespace Sophia.Data.World;

public class WorldRepository(DataContext dataContext, DbContext dbContext)
    : EntityFrameworkRepository<WorldData, Guid, WorldEntity, Guid>(dataContext, dbContext) {
    //protected override Expression<Func<WorldEntity, WorldData>> Project { get; }
    //    = input => Mapper.ToWorldData(input);

    //protected override Action<WorldData, WorldEntity> UpdateFrom { get; }
    //    = Mapper.UpdateWorldEntity;
}
