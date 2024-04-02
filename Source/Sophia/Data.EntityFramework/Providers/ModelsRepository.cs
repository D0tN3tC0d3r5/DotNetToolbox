namespace Sophia.Data.Providers;

public class ModelsRepository(ApplicationDbContext dbContext)
    : EntityFrameworkRepository<ModelData, ModelEntity, string>(dbContext.Models) {
    protected override Expression<Func<ModelEntity, ModelData>> ProjectTo { get; }
        = input => Mapper.ToModelData(input, true);
    protected override Action<ModelData, ModelEntity> UpdateFrom { get; }
        = Mapper.UpdateModelEntity;
    protected override Func<ModelData, ModelEntity> CreateFrom { get; }
        = Mapper.ToModelEntity;
}
