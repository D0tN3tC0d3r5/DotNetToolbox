namespace Sophia.WebApp.Client.Pages.Settings.WorldPageModel;

public class WorldData(DotNetToolbox.AI.Chats.World world) {
    public DateTimeOffset DateTime { get; set; } = world.DateTime;
    public string? Location { get; set; } = world.Location;
    public string? UserName { get; set; } = world.UserName;
    public List<InformationData> CustomValues { get; set; } = world.CustomValues.ToList(x => new InformationData(x));
    public List<SkillData> Skills { get; set; } = world.Skills.ToList(x => new SkillData(x));

    public DotNetToolbox.AI.Chats.World ToWorld()
        => new() {
            Location = Location,
            UserName = UserName,
            CustomValues = CustomValues.ToList(x => x.ToWorld()),
            Skills = Skills.ToList(x => x.ToWorld()),
        };
}
