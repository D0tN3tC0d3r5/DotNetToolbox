namespace Sophia.Data.Tools;

public class ToolsRepository(ApplicationDbContext dbContext)
    : EntityFrameworkRepository<ToolData, ToolEntity, int>(dbContext.Tools) {
    protected override Expression<Func<ToolEntity, ToolData>> ProjectTo { get; }
        = input => Mapper.ToToolData(input);
    protected override Action<ToolData, ToolEntity> UpdateFrom { get; }
        = Mapper.UpdateToolEntity;
    protected override Func<ToolData, ToolEntity> CreateFrom { get; }
        = Mapper.ToToolEntity;
}
