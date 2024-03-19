using Sophia.Models.Tools;
using Sophia.WebApp.Data.Tools;

namespace Sophia.WebApp.Data;

public static class Mapper {
    public static WorldEntity ToEntity(this WorldData input)
        => new() {
            DateTime = input.DateTime,
            Location = input.Location,
            UserProfile = input.UserProfile,
        };

    public static ToolEntity ToEntity(this ToolData input)
        => new() {
            Name = input.Name,
            Description = input.Description,
        };

    public static ArgumentEntity ToEntity(this ArgumentData input, uint index)
        => new() {
            Index = index,
            Name = input.Name,
            Description = input.Description,
            Type = input.Type,
            Choices = input.Type != ArgumentType.Enum ? [] : [.. input.Choices],
            IsRequired = input.IsRequired,
        };

    public static InformationEntity ToEntity(this InformationData input)
        => new() {
            DefaultText = input.DefaultText,
            Value = input.Value,
            ValueTemplate = input.ValueTemplate,
        };

    public static void UpdateFrom(this ToolEntity target, ToolData input) {
        target.Name = input.Name;
        target.Description = input.Description;
        target.Arguments = input.Arguments.AsIndexed().ToList(i => i.Value.ToEntity(i.Index));
    }
}
