namespace Sophia.Data.Tools;

public class ToolsRepository(DataContext dataContext, ApplicationDbContext dbContext)
    : EntityFrameworkRepository<ToolData, ToolEntity, int>(dataContext, dbContext.Tools) {
    protected override Expression<Func<ToolEntity, ToolData>> Project { get; }
        = input => Mapper.ToToolData(input);
    protected override Action<ToolData, ToolEntity> UpdateFrom { get; }
        = Mapper.UpdateToolEntity;
    protected override Func<ToolData, ToolEntity> Create { get; }
        = Mapper.ToToolEntity;
}
