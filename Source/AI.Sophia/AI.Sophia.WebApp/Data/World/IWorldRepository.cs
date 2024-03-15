namespace Sophia.WebApp.Data.World;

public interface IWorldRepository {
    DotNetToolbox.AI.Chats.World GetWorld();
    void UpdateWorld(DotNetToolbox.AI.Chats.World world);
}
