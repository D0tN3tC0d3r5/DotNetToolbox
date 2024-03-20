namespace Sophia.Services;

public interface IWorldRemoteService : IWorldService;
public interface IWorldService {
    Task<WorldData> GetWorld();
    Task UpdateWorld(WorldData world);
}
