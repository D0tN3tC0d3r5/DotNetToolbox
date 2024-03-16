namespace Sophia.WebApp.Services;

public class WorldService(IWorldRepository worldRepository)
    : IWorldService {
    public Task<WorldData> GetWorld() => worldRepository.GetWorld();

    public Task UpdateWorld(WorldData input) => worldRepository.UpdateWorld(input);
}
