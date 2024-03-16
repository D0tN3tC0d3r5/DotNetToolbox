namespace Sophia.Services.WorldServiceModel;

public class WorldData {
    public DateTimeOffset DateTime { get; set; }
    public string? Location { get; set; }
    public string? UserProfile { get; set; }
    public List<InformationData> AdditionalInformation { get; set; } = [];
    public List<SkillData> Skills { get; set; } = [];

    public World GetUpdateModel(World world) {
        world.Location = Location;
        world.UserProfile = UserProfile;
        world.AdditionalInformation = AdditionalInformation.ToList(x => x.ToModel());
        world.Skills = Skills.ToList(x => x.ToModel(world.Skills));
        return world;
    }
}
