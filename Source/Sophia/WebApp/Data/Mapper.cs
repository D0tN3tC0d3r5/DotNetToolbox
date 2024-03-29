using IHasMessages = Sophia.WebApp.Data.Chats.IHasMessages;

namespace Sophia.WebApp.Data;

public static class Mapper {
    public static void UpdateFrom(this WorldEntity target, WorldData input)
        => target.Facts = input.Facts;

    public static ProviderEntity ToEntity(this ProviderData input)
        => new() {
            Id = input.Id,
            Name = input.Name,
            Models = input.Models.ToList(i => i.ToEntity()),
        };

    public static void UpdateFrom(this ProviderEntity target, ProviderData input) {
        target.Id = input.Id;
        target.Name = input.Name;
        target.Models = input.Models.ToList(i => i.AddOrUpdate(target));
    }

    public static ModelEntity ToEntity(this ModelData input)
        => new() {
            ModelId = input.Id,
            Name = input.Name,
        };

    public static void UpdateFrom(this ModelEntity target, ModelData input) {
        target.ModelId = input.Id;
        target.Name = input.Name;
    }

    public static ModelEntity AddOrUpdate(this ModelData input, IHasModels parent) {
        var originalItem = parent.Models.FirstOrDefault(i => i.ModelId == input.Id);
        if (originalItem is null) return input.ToEntity();
        originalItem.UpdateFrom(input);
        return originalItem;
    }

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
            IsActive = input.IsActive,
            Title = input.Title,
            Agents = input.Agents.ToList(i => i.ToEntity()),
            Instructions = input.Instructions.ToEntity(),
            Messages = input.Messages.ToList(i => i.ToEntity()),
        };

    public static InstructionsEntity ToEntity(this InstructionsData input)
        => new() {
                     Goals = input.Goals,
                     Requirements = input.Requirements,
                     Assumptions = input.Assumptions,
                     Constraints = input.Constraints,
                     Examples = input.Examples,
                     Validation = input.Validation,
                 };


    public static ChatAgentEntity ToEntity(this ChatAgentData input)
        => new() {
            ChatId = input.ChatId,
            Number = input.AgentNumber,
            PersonaId = input.Persona.Id,
            Persona = input.Persona.ToEntity(),
            ModelId = input.Options.Id,
            Options = input.Options.ToEntity(),
            Messages = input.Messages.ToList(i => i.ToEntity()),
        };

    public static ChatAgentOptionsEntity ToEntity(this ChatAgentOptionsData input)
        => new() {
                     ModelId = input.Model.Id,
                     NumberOfRetries = input.NumberOfRetries,
                     MaximumOutputTokens = input.MaximumOutputTokens,
                     Temperature = input.Temperature,
                     TokenProbabilityCutOff = input.TokenProbabilityCutOff,
                     StopSequences = [.. input.StopSequences],
                     IsStreaming = input.IsStreaming,
                     RespondsAsJson = input.RespondsAsJson,
                 };

    public static MessageEntity ToEntity(this MessageData input, IHasMessages? parent = null)
        => new() {
            ChatId = parent switch {
                IHasChatMessages hcm => hcm.Id,
                IHasAgentMessages ham => ham.ChatId,
                _ => default!,
            },
            AgentNumber = parent switch {
                IHasAgentMessages ham => ham.Number,
                _ => null,
            },
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
            Conduct = input.Conduct,
            Facts = input.Facts,
            Tools = input.KnownTools.ToList(i => i.ToEntity()),

        };

    public static void UpdateFrom(this PersonaEntity target, PersonaData input) {
        target.Name = input.Name;
        target.Description = input.Description;
        target.Personality = input.Personality;
        target.Conduct = input.Conduct;
        target.Facts = input.Facts;
        target.Tools = input.KnownTools.ToList(i => i.ToEntity());
    }
}
