namespace Sophia.Data.Tools;

public class ToolsRepository(DataContext dataContext, DbContext dbContext)
    : EntityFrameworkRepository<ToolData, int, ToolEntity, int>(dataContext, dbContext) {
    //protected override Expression<Func<ToolEntity, ToolData>> Project { get; }
    //    = input => Mapper.ToToolData(input);

    //protected override Action<ToolData, ToolEntity> UpdateFrom { get; }
    //    = Mapper.UpdateToolEntity;
}
