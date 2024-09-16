using DotNetToolbox.Data.Storages;

namespace Lola.Models.Repositories;

public interface IModelStorage
    : IStorage<ModelEntity, string>;
