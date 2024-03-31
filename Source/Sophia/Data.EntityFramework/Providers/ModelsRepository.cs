namespace Sophia.Data.Providers;

public class ModelsRepository(DataContext dataContext, DbContext dbContext)
    : EntityFrameworkRepository<ModelData, string, ModelEntity, string>(dataContext, dbContext) {
    //protected override Expression<Func<ModelEntity, ModelData>> Project { get; }
    //    = input => Mapper.ToModelData(input);

    //protected override Action<ModelData, ModelEntity> UpdateFrom { get; }
    //    = Mapper.UpdateModelEntity;
}
