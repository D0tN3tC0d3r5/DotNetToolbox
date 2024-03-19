using Sophia.Models.Tools;

namespace Sophia.WebApp.Services;

public static class Mapper {
    public static WorldData ToDto(this World input)
        => new() {
            DateTime = input.DateTime,
            Location = input.Location,
            UserProfile = input.UserProfile,
            AdditionalInformation = input.AdditionalInformation.ToList(i => i.ToDto()),
            AvailableTools = input.AvailableTools.ToList(i => i.ToDto()),
        };

    public static World ToModel(this WorldData input, IDateTimeProvider? dateTime)
        => new(dateTime) {
            Location = input.Location,
            UserProfile = input.UserProfile,
            AdditionalInformation = input.AdditionalInformation.ToList(i => i.ToModel()),
            AvailableTools = input.AvailableTools.ToList(i => i.ToModel()),
        };

    public static void UpdateFrom(this World target, WorldData input) {
        target.Location = input.Location;
        target.UserProfile = input.UserProfile;
        target.AdditionalInformation = input.AdditionalInformation.ToList(i => i.ToModel());
        target.AvailableTools = input.AvailableTools.ToList(i => i.ToModel());
    }

    public static ToolData ToDto(this Tool input)
        => new() {
            Id = input.Id,
            Name = input.Name,
            Description = input.Description,
            Arguments = input.Arguments.ToList(i => i.ToDto()),
        };

    public static void ToModel(this Tool target, ToolData input) {
        target.Id = input.Id;
        target.Name = input.Name;
        target.Description = input.Description;
        target.Arguments = input.Arguments.AsIndexed().ToList(i => i.Value.ToModel(i.Index));
    }

    public static Tool ToModel(this ToolData input)
        => new() {
            Id = input.Id,
            Name = input.Name,
            Description = input.Description,
            Arguments = input.Arguments.AsIndexed().ToList(i => i.Value.ToModel(i.Index)),
        };

    public static ArgumentData ToDto(this Argument input)
        => new() {
            Name = input.Name,
            Description = input.Description,
            Type = input.Type,
            Choices = input.Options?.ToList() ?? [],
            IsRequired = input.IsRequired,
        };

    public static Argument ToModel(this ArgumentData input, uint index)
        => new() {
            Index = index,
            Name = input.Name,
            Description = input.Description,
            Type = input.Type,
            Options = input.Type != ArgumentType.Enum ? null : [.. input.Choices],
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
