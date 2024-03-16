namespace Sophia.WebApp.Data.Worlds;

public static class WorldDataExtensions {
    public static WorldEntity ToEntity(this WorldData input)
        => new() {
            DateTime = input.DateTime,
            Location = input.Location,
            UserProfile = input.UserProfile,
            CustomValues = input.AdditionalInformation.ToList(i => i.ToEntity()),
            Skills = input.Skills.ToList(i => i.ToEntity()),
        };

    public static SkillEntity ToEntity(this SkillData input)
        => new() {
            Id = input.Id,
            Name = input.Name,
            Description = input.Description,
            Arguments = input.Arguments.ToList(i => i.ToEntity()),
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
}
