namespace Sophia.Models.Worlds;

public class WorldData {
    public DateTimeOffset DateTime { get; set; }
    public string? Location { get; set; }
    public string? UserProfile { get; set; }
    public List<InformationData> AdditionalInformation { get; set; } = [];
    public List<ToolData> AvailableTools { get; set; } = [];

    public World GetUpdateModel(World world) {
        world.Location = Location;
        world.UserProfile = UserProfile;
        world.AdditionalInformation = AdditionalInformation.ToList(x => x.ToModel());
        world.AvailableTools = AvailableTools.ToList(x => x.ToModel(world.AvailableTools));
        return world;
    }
}
