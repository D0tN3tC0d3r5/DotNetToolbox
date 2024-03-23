namespace Sophia.WebApp.Data;

public static class Mapper {
    public static void UpdateFrom(this WorldEntity target, WorldData input) {
        target.Location = input.Location;
        target.UserProfile = input.UserProfile.ToEntity();
        target.Facts = input.Facts.ToList(i => i.ToEntity());
        target.Tools = input.Tools.ToList(i => i.AddOrUpdate(target));
    }

    public static ProviderEntity ToEntity(this ProviderData input)
        => new() {
            Id = input.Id,
            Name = input.Name,
            Authentication = input.Authentication,
            Models = input.Models.ToList(i => i.ToEntity()),
        };

    public static void UpdateFrom(this ProviderEntity target, ProviderData input) {
        target.Id = input.Id;
        target.Name = input.Name;
        target.Authentication = input.Authentication;
        target.Models = input.Models.ToList(i => i.AddOrUpdate(target));
    }

    public static ModelEntity ToEntity(this ModelData input)
        => new() {
            Key = input.Key,
            Name = input.Name,
        };

    public static void UpdateFrom(this ModelEntity target, ModelData input) {
        target.Key = input.Key;
        target.Name = input.Name;
    }

    public static ModelEntity AddOrUpdate(this ModelData input, IHasModels parent) {
        var originalItem = parent.Models.FirstOrDefault(i => i.Key == input.Key);
        if (originalItem is null) return input.ToEntity();
        originalItem.UpdateFrom(input);
        return originalItem;
    }

    public static FactEntity ToEntity(this FactData input)
        => new() {
            DefaultText = input.DefaultText,
            Value = input.Value,
            ValueTemplate = input.ValueTemplate,
        };

    public static ApplicationUserProfile ToEntity(this UserProfileData input)
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

    public static ToolEntity AddOrUpdate<TKey>(this ToolData input, IHasTools<TKey> parent) {
        var originalTool = parent.Tools.FirstOrDefault(i => i.Id == input.Id);
        if (originalTool is null) return input.ToEntity();
        originalTool.UpdateFrom(input);
        return originalTool;
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

    public static ChatEntity ToEntity(this ChatData input)
        => new() {
            Id = Guid.NewGuid().ToString(),
            IsActive = input.IsActive,
            Title = input.Title,
            Model = input.Agent.Model,
            PersonaId = input.Agent.Persona.Id,
            Temperature = input.Agent.Temperature,
            Messages = input.Messages.ToList(i => i.ToEntity()),
        };

    public static MessageEntity ToEntity(this MessageData input, IHasMessages? parent = null)
        => new() {
            ChatId = parent?.Id ?? default!,
            Index = parent?.Messages.Count ?? 0,
            Content = input.Content,
            Type = input.Type,
            Timestamp = input.Timestamp,
        };

    public static void UpdateFrom(this MessageEntity target, MessageData input) {
        target.Content = input.Content;
        target.Type = input.Type;
        target.Timestamp = input.Timestamp;
    }

    public static MessageEntity AddOrUpdate(this MessageData input, IHasMessages parent) {
        var originalItem = parent.Messages.FirstOrDefault(i => i.Index == input.Index);
        if (originalItem is null) return input.ToEntity(parent);
        originalItem.UpdateFrom(input);
        return originalItem;
    }

    public static PersonaEntity ToEntity(this PersonaData input)
        => new() {
            Name = input.Name,
            Description = input.Description,
            Personality = input.Personality,
            Instructions = [.. input.Instructions],
            Facts = input.Facts.ToList(i => i.ToEntity()),
            Tools = input.KnownTools.ToList(i => i.ToEntity()),

        };

    public static void UpdateFrom(this PersonaEntity target, PersonaData input) {
        target.Name = input.Name;
        target.Description = input.Description;
        target.Personality = input.Personality;
        target.Instructions = [.. input.Instructions];
        target.Facts = input.Facts.ToList(i => i.ToEntity());
        target.Tools = input.KnownTools.ToList(i => i.ToEntity());
    }
}
