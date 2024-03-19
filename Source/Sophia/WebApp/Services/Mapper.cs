namespace Sophia.WebApp.Services;

public static class Mapper {
    public static void UpdateFrom(this World target, WorldData input) {
        target.Location = input.Location;
        target.UserProfile = input.UserProfile.ToModel();
        target.Facts = input.Facts.ToList(i => i.ToModel());
        target.AvailableTools = input.Tools.ToList(i => i.ToModel());
    }

    public static WorldData ToDto(this World input)
        => new() {
            DateTime = input.DateTime,
            Location = input.Location,
            UserProfile = input.UserProfile.ToDto(),
            Facts = input.Facts.ToList(i => i.ToDto()),
            Tools = input.AvailableTools.ToList(i => i.ToDto()),
        };

    public static ToolData ToDto(this Tool input)
        => new() {
            Id = input.Id,
            Name = input.Name,
            Description = input.Description,
            Arguments = input.Arguments.ToList(i => i.ToDto()),
        };

    public static ArgumentData ToDto(this Argument input)
        => new() {
            Name = input.Name,
            Description = input.Description,
            Type = input.Type,
            Choices = input.Options?.ToList() ?? [],
            IsRequired = input.IsRequired,
        };

    public static FactData ToDto(this Fact input)
        => new() {
            DefaultText = input.DefaultText,
            Value = input.Value,
            ValueTemplate = input.ValueTemplate,
        };

    public static UserProfileData ToDto(this UserProfile input)
        => new() {
            Name = input.Name,
            Language = input.Language,
        };
}
