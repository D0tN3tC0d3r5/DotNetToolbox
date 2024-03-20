namespace Sophia.WebApp.Data;

public static class Mapper {
    public static WorldEntity ToEntity(this WorldData input)
        => new() {
            Location = input.Location,
            UserProfile = input.UserProfile.ToEntity(),
            Facts = input.Facts.ToList(i => i.ToEntity()),
            Tools = input.Tools.ToList(i => i.ToEntity()),
        };
    public static void UpdateFrom(this WorldEntity target, WorldData input) {
        target.Location = input.Location;
        target.UserProfile = input.UserProfile.ToEntity();
        target.Facts = input.Facts.ToList(i => i.ToEntity());
        target.Tools = input.Tools.ToList(i => i.AddOrUpdate(target.Tools));
    }

    public static FactEntity ToEntity(this FactData input)
        => new() {
                     DefaultText = input.DefaultText,
                     Value = input.Value,
                     ValueTemplate = input.ValueTemplate,
                 };

    public static UserProfileEntity ToEntity(this UserProfileData input)
        => new() {
            Name = input.Name,
            Language = input.Language,
        };

    public static ToolEntity ToEntity(this ToolData input)
        => new() {
            Name = input.Name,
            Description = input.Description,
            Arguments = input.Arguments.AsIndexed().ToList(i => i.Value.ToEntity(i.Index)),
        };

    public static void UpdateFrom(this ToolEntity target, ToolData input) {
        target.Name = input.Name;
        target.Description = input.Description;
        target.Arguments = input.Arguments.AsIndexed().ToList(i => i.Value.ToEntity(i.Index));
    }

    public static ToolEntity AddOrUpdate(this ToolData input, IReadOnlyList<ToolEntity> originalItems) {
        var originalItem = originalItems.FirstOrDefault(i => i.Id == input.Id);
        if (originalItem is null) return input.ToEntity();
        originalItem.UpdateFrom(input);
        return originalItem;
    }

    public static ArgumentEntity ToEntity(this ArgumentData input, int index)
        => new() {
            Index = index,
            Name = input.Name,
            Description = input.Description,
            Type = input.Type,
            Choices = input.Type != ArgumentType.Enum ? [] : [.. input.Choices],
            IsRequired = input.IsRequired,
        };

    public static PersonaEntity ToEntity(this PersonaData input)
        => new() {
            Name = input.Name,
            Description = input.Description,
            Personality = input.Personality,
            Instructions = [..input.Instructions],
            Facts = input.Facts.ToList(i => i.ToEntity()),
            KnownTools = input.KnownTools.ToList(i => i.ToEntity()),

        };

    public static void UpdateFrom(this PersonaEntity target, PersonaData input) {
        target.Name = input.Name;
        target.Description = input.Description;
        target.Personality = input.Personality;
        target.Instructions = [..input.Instructions];
        target.Facts = input.Facts.ToList(i => i.ToEntity());
        target.KnownTools = input.KnownTools.ToList(i => i.ToEntity());
    }
}
