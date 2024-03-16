namespace Sophia.WebApp.Data.World;

public static class WorldExtensions {
    public static WorldEntity ToEntity(this WorldData input)
        => new() {
            DateTime = input.DateTime,
            Location = input.Location,
            UserProfile = input.UserProfile,
        };

    public static SkillEntity ToEntity(this SkillData input)
        => new() {
            Name = input.Name,
            Description = input.Description,
        };

    public static ArgumentEntity ToEntity(this ArgumentData input)
        => new() {
            Name = input.Name,
            Description = input.Description,
            Type = input.Type,
            Options = input.Options,
            IsRequired = input.IsRequired,
        };

    public static InformationEntity ToEntity(this InformationData input)
        => new() {
            DefaultText = input.DefaultText,
            Value = input.Value,
            ValueTemplate = input.ValueTemplate,
        };

    public static void UpdateFrom(this SkillEntity target, SkillData input) {
        target.Name = input.Name;
        target.Description = input.Description;
        target.Arguments = input.Arguments.ToList(i => i.ToEntity());
    }
}
