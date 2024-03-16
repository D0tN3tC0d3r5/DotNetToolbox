namespace Sophia.Services.Repositories;

public interface IWorldRepository {
    Task<WorldData> GetWorld();
    Task UpdateWorld(WorldData world);
}
