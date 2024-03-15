namespace Sophia.WebApp.Client.Pages.Settings.WorldPageModel;

public class SkillData(Skill skill) {
    public string Name { get; set; } = skill.Name;
    public string? Description { get; set; } = skill.Description;
    public List<ArgumentData> Arguments { get; set; } = skill.Arguments.ToList(x => new ArgumentData(x));

    public Skill ToWorld()
        => new(Name, Description, Arguments.ToList(x => x.ToWorld()));
}
