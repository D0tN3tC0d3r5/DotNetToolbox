using Sophia.Models.Worlds;

namespace Sophia.Repositories;

public interface IWorldRepository {
    Task<WorldData> GetWorld();
    Task UpdateWorld(WorldData world);
}
