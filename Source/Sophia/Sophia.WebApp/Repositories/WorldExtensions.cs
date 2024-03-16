namespace Sophia.WebApp.Repositories;

public static class WorldExtensions {
    public static WorldData ToDto(this World input)
        => new() {
            DateTime = input.DateTime,
            Location = input.Location,
            UserProfile = input.UserProfile,
            AdditionalInformation = input.AdditionalInformation.ToList(i => i.ToDto()),
            Skills = input.Skills.ToList(i => i.ToDto()),
        };

    public static World ToModel(this WorldData input, IDateTimeProvider? dateTime)
        => new(dateTime) {
            Location = input.Location,
            UserProfile = input.UserProfile,
            AdditionalInformation = input.AdditionalInformation.ToList(i => i.ToModel()),
            Skills = input.Skills.ToList(i => i.ToModel()),
        };

    public static void UpdateFrom(this World target, WorldData input) {
        target.Location = input.Location;
        target.UserProfile = input.UserProfile;
        target.AdditionalInformation = input.AdditionalInformation.ToList(i => i.ToModel());
        target.Skills = input.Skills.ToList(i => i.ToModel());
    }

    public static SkillData ToDto(this Skill input)
        => new() {
            Id = input.Id,
            Name = input.Name,
            Description = input.Description,
            Arguments = input.Arguments.ToList(i => i.ToDto()),
        };

    public static Skill ToModel(this SkillData input)
        => new() {
            Id = input.Id,
            Name = input.Name,
            Description = input.Description,
            Arguments = input.Arguments.ToList(i => i.ToModel()),
        };

    public static ArgumentData ToDto(this Argument input)
        => new() {
            Name = input.Name,
            Description = input.Description,
            Type = input.Type,
            Options = input.Options,
            IsRequired = input.IsRequired,
        };

    public static Argument ToModel(this ArgumentData input)
        => new() {
            Name = input.Name,
            Description = input.Description,
            Type = input.Type,
            Options = input.Options,
            IsRequired = input.IsRequired,
        };

    public static InformationData ToDto(this Information input)
        => new() {
            DefaultText = input.DefaultText,
            Value = input.Value,
            ValueTemplate = input.ValueTemplate,
        };

    public static Information ToModel(this InformationData input)
        => new() {
            DefaultText = input.DefaultText,
            Value = input.Value,
            ValueTemplate = input.ValueTemplate,
        };
}
