namespace Sophia.Data;

public static class Mapper {
    internal static ChatAgentData ToChatAgentData(ChatAgentEntity input)
        => new() {
            Persona = ToPersonaData(input.Persona),
            Model = ToModelData(input.Model),
            Options = ToChatAgentOptionsData(input.Options),
            Messages = input.Messages.ToList(ToMessageData),
        };

    internal static ChatAgentOptionsData ToChatAgentOptionsData(ChatAgentOptionsEntity input)
        => new() {
            NumberOfRetries = input.NumberOfRetries,
            MaximumOutputTokens = input.MaximumOutputTokens,
            Temperature = input.Temperature,
            TokenProbabilityCutOff = input.TokenProbabilityCutOff,
            StopSequences = [.. input.StopSequences],
            IsStreaming = input.IsStreaming,
            RespondsAsJson = input.RespondsAsJson,
        };

    internal static MessageData ToMessageData(MessageEntity input)
        => new() {
            Timestamp = input.Timestamp,
            Type = input.Type,
            Content = input.Content,
        };

    internal static ModelData ToModelData(ModelEntity input)
        => new() {
            Id = input.ModelId,
            Name = input.Name,
        };

    internal static InstructionsData ToInstructionsData(InstructionsEntity input)
        => new() {
            Goals = input.Goals,
            Scope = input.Scope,
            Requirements = input.Requirements,
            Assumptions = input.Assumptions,
            Constraints = input.Constraints,
            Examples = input.Examples,
            Strategy = input.Strategy,
            Evaluation = input.Validation,
        };

    internal static PersonaData ToPersonaData(PersonaEntity input) => new() {
        Id = input.Id,
        Name = input.Name,
        Description = input.Description,
        Characteristics = ToCharacteristicsData(input.Characteristics),
        Facts = input.Facts,
        KnownTools = input.Tools.ToList(ToToolData),
    };

    internal static CharacteristicsData ToCharacteristicsData(CharacteristicsEntity input)
        => new() {
            Cognition = input.Cognition,
            Disposition = input.Disposition,
            Interaction = input.Interaction,
            Attitude = input.Attitude,
        };

    internal static ToolData ToToolData(ToolEntity input)
        => new() {
            Name = input.Name,
            Description = input.Description,
            Arguments = input.Arguments.AsIndexed().ToList(ToArgumentData),
        };

    internal static ArgumentData ToArgumentData(Indexed<ArgumentEntity> input)
        => new() {
            Index = input.Index,
            Name = input.Value.Name,
            Description = input.Value.Description,
            Type = input.Value.Type,
            Choices = input.Value.Type != ArgumentType.Enum ? [] : [.. input.Value.Choices],
            IsRequired = input.Value.IsRequired,
        };

    internal static ToolEntity ToToolEntity(ToolData input)
        => new() {
            Name = input.Name,
            Description = input.Description,
            Arguments = input.Arguments.AsIndexed().ToList(ToArgumentEntity),
        };

    internal static ArgumentEntity ToArgumentEntity(Indexed<ArgumentData> input)
        => new() {
            Index = input.Index,
            Name = input.Value.Name,
            Description = input.Value.Description,
            Type = input.Value.Type,
            Choices = input.Value.Type != ArgumentType.Enum ? [] : [.. input.Value.Choices],
            IsRequired = input.Value.IsRequired,
        };

    internal static ChatData ToChatData(ChatEntity input)
        => new() {
            Id = input.Id,
            Title = input.Title,
            IsActive = input.IsActive,
            Agents = input.Agents.ToList(ToChatAgentData),
            Instructions = ToInstructionsData(input.Instructions),
            Messages = input.Messages.ToList(ToMessageData),
        };

    internal static ChatAgentEntity ToChatAgentEntity(ChatAgentData input)
        => new() {
            ModelId = input.Model.Id,
            ChatId = input.ChatId,
            Number = input.Number,
            PersonaId = input.Persona.Id,
            Options = ToChatAgentOptionsEntity(input.Options),
            Messages = input.Messages.ToList(i => ToMessageEntity(i, input)),
        };

    internal static ChatAgentOptionsEntity ToChatAgentOptionsEntity(ChatAgentOptionsData input)
        => new() {
            NumberOfRetries = input.NumberOfRetries,
            MaximumOutputTokens = input.MaximumOutputTokens,
            Temperature = input.Temperature,
            TokenProbabilityCutOff = input.TokenProbabilityCutOff,
            StopSequences = [.. input.StopSequences],
            IsStreaming = input.IsStreaming,
            RespondsAsJson = input.RespondsAsJson,
        };

    internal static MessageEntity ToMessageEntity(MessageData input, IHasMessages? parent = null)
        => new() {
            ChatId = parent switch {
                IHasChatMessages hcm => hcm.Id,
                IHasChatAgentMessages ham => ham.ChatId,
                _ => default!,
            },
            AgentNumber = parent switch {
                IHasChatAgentMessages ham => ham.Number,
                _ => null,
            },
            Index = parent?.Messages.Count ?? 0,
            Content = input.Content,
            Type = input.Type,
            Timestamp = input.Timestamp,
        };

    internal static ProviderEntity ToProviderEntity(ProviderData input)
        => new() {
            Id = input.Id,
            Name = input.Name,
            Models = input.Models.ToList(ToModelEntity),
        };

    internal static ModelEntity ToModelEntity(ModelData input)
        => new() {
            ModelId = input.Id,
            Name = input.Name,
        };

    public static ChatEntity ToChatEntity(ChatData input)
        => new() {
            IsActive = input.IsActive,
            Title = input.Title,
            Agents = input.Agents.ToList(ToChatAgentEntity),
            Instructions = ToInstructionsEntity(input.Instructions),
            Messages = input.Messages.ToList(i => ToMessageEntity(i)),
        };

    public static InstructionsEntity ToInstructionsEntity(InstructionsData input)
        => new() {
            Goals = input.Goals
                                  .Where(p => !string.IsNullOrWhiteSpace(p))
                                  .Distinct()
                                  .ToList(),
            Scope = input.Scope
                                  .Where(p => !string.IsNullOrWhiteSpace(p))
                                  .Distinct()
                                  .ToList(),
            Requirements = input.Requirements
                                         .Where(p => !string.IsNullOrWhiteSpace(p))
                                         .Distinct()
                                         .ToList(),
            Assumptions = input.Assumptions
                                        .Where(p => !string.IsNullOrWhiteSpace(p))
                                        .Distinct()
                                        .ToList(),
            Constraints = input.Constraints
                                        .Where(p => !string.IsNullOrWhiteSpace(p))
                                        .Distinct()
                                        .ToList(),
            Examples = input.Examples
                                     .Where(p => !string.IsNullOrWhiteSpace(p))
                                     .Distinct()
                                     .ToList(),
            Strategy = input.Strategy
                                     .Where(p => !string.IsNullOrWhiteSpace(p))
                                     .Distinct()
                                     .ToList(),
            Validation = input.Evaluation
                                       .Where(p => !string.IsNullOrWhiteSpace(p))
                                       .Distinct()
                                       .ToList(),
        };

    internal static CharacteristicsEntity ToCharacteristicsEntity(CharacteristicsData input)
        => new() {
            Cognition = input.Cognition
                                      .Where(p => !string.IsNullOrWhiteSpace(p))
                                      .Distinct()
                                      .ToList(),
            Disposition = input.Disposition
                                        .Where(p => !string.IsNullOrWhiteSpace(p))
                                        .Distinct()
                                        .ToList(),
            Interaction = input.Interaction
                                        .Where(p => !string.IsNullOrWhiteSpace(p))
                                        .Distinct()
                                        .ToList(),
            Attitude = input.Attitude
                                     .Where(p => !string.IsNullOrWhiteSpace(p))
                                     .Distinct()
                                     .ToList(),
        };

    internal static PersonaEntity ToPersonaEntity(PersonaData input)
        => new() {
            Name = input.Name,
            Description = input.Description,
            Characteristics = ToCharacteristicsEntity(input.Characteristics),
            Facts = input.Facts
                                  .Where(p => !string.IsNullOrWhiteSpace(p))
                                  .Distinct().
                                   ToList(),
            Tools = input.KnownTools.ToList(ToToolEntity),
        };

    //public static void UpdateFrom(MessageEntity target, MessageData input) {
    //    target.Content = input.Content;
    //    target.Type = input.Type;
    //    target.Timestamp = input.Timestamp;
    //}

    //public static MessageEntity AddOrUpdate(MessageData input, IHasMessages parent) {
    //    var originalItem = parent.Messages.FirstOrDefault(i => i.Index == input.Index);
    //    if (originalItem is null) return input.ToEntity(parent);
    //    originalItem.UpdateFrom(input);
    //    return originalItem;
    //}

    //public static void UpdateFrom(PersonaEntity target, PersonaData input) {
    //    target.Name = input.Name;
    //    target.Description = input.Description;
    //    target.Characteristics = input.Characteristics.ToEntity();
    //    target.Facts = input.Facts
    //                        .Where(p => !string.IsNullOrWhiteSpace(p))
    //                        .Distinct()
    //                        .ToList();
    //    target.Tools = input.KnownTools.ToList(i => i.ToEntity());
    //}
}
