using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace Sophia.Data;

public static class Mapper {
    internal const DynamicallyAccessedMemberTypes AccessedMembers =
        PublicConstructors | NonPublicConstructors
                           | PublicProperties | NonPublicProperties
                           | PublicFields | NonPublicFields
                           | Interfaces;

    #region ProjectTo To Domain Model
    internal static UserData ToUserData(User input)
        => new() {
            Id = input.Id,
            Name = input.Name,
            Language = input.Language,
            Facts = input.Facts,
        };

    internal static WorldData ToWorldData(WorldEntity input)
        => new() {
            Id = input.Id,
            Facts = input.Facts,
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

    internal static ChatAgentData ToChatAgentData(ChatAgentEntity input)
        => new() {
            Persona = ToPersonaData(input.Persona),
            Model = ToModelData(input.Model, true),
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

    internal static ModelData ToModelData(ModelEntity input, bool includeProvider)
        => new() {
            Id = input.Id,
            Name = input.Name,
            Provider = includeProvider ? ToProviderData(input.Provider, false) : null!,
        };

    internal static ProviderData ToProviderData(ProviderEntity input, bool includeModels)
        => new() {
            Id = input.Id,
            Name = input.Name,
            Models = includeModels ? input.Models.ToList(m => ToModelData(m, false)) : [],
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

    #endregion
    #region Update Entity
    public static void UpdateUser(UserData input, User target) {
        target.Name = input.Name;
        target.Language = input.Language;
        target.Facts = ToDistinctList(input.Facts);
    }

    public static void UpdateWorldEntity(WorldData input, WorldEntity target)
        => target.Facts = ToDistinctList(input.Facts);

    public static void UpdateChatEntity(ChatData input, ChatEntity target) {
        target.IsActive = input.IsActive;
        target.Title = input.Title;
        UpdateChatAgentEntityList(input.Agents, target.Agents);
        UpdateInstructionsEntity(input.Instructions, target.Instructions);
        UpdateMessageEntityList(input.Messages, target.Messages);
    }

    private static void UpdateChatAgentEntityList(IEnumerable<ChatAgentData> inputList, ICollection<ChatAgentEntity> targetList) {
        targetList.Clear();
        foreach (var input in inputList)
            targetList.Add(ToChatAgentEntity(input));
    }

    internal static void UpdateChatAgentEntity(ChatAgentData input, ChatAgentEntity target) {
        target.ModelId = input.Model.Id;
        target.PersonaId = input.Persona.Id;
        UpdateChatAgentOptionsEntity(input.Options, target.Options);
        UpdateMessageEntityList(input.Messages, target.Messages);
    }

    internal static void UpdateChatAgentOptionsEntity(ChatAgentOptionsData input, ChatAgentOptionsEntity target) {
        target.NumberOfRetries = input.NumberOfRetries;
        target.MaximumOutputTokens = input.MaximumOutputTokens;
        target.Temperature = input.Temperature;
        target.TokenProbabilityCutOff = input.TokenProbabilityCutOff;
        target.StopSequences = input.StopSequences;
        target.IsStreaming = input.IsStreaming;
        target.RespondsAsJson = input.RespondsAsJson;
    }

    private static void UpdateMessageEntityList(IEnumerable<MessageData> inputList, ICollection<MessageEntity> targetList) {
        targetList.Clear();
        foreach (var input in inputList)
            targetList.Add(ToMessageEntity(input));
    }

    internal static void UpdateMessageEntity(MessageData input, MessageEntity target) {
        target.Index = input.Index;
        target.Timestamp = input.Timestamp;
        target.Type = input.Type;
        target.Content = input.Content;
    }

    private static void UpdateModelEntityList(IEnumerable<ModelData> inputList, ICollection<ModelEntity> targetList) {
        targetList.Clear();
        foreach (var input in inputList)
            targetList.Add(ToModelEntity(input));
    }

    internal static void UpdateModelEntity(ModelData input, ModelEntity target) {
        target.Id = input.Id;
        target.Name = input.Name;
    }

    public static void UpdateProviderEntity(ProviderData input, ProviderEntity target) {
        target.Name = input.Name;
        UpdateModelEntityList(input.Models, target.Models);
    }

    internal static void UpdateInstructionsEntity(InstructionsData input, InstructionsEntity target) {
        target.Goals = ToDistinctList(input.Goals);
        target.Scope = ToDistinctList(input.Scope);
        target.Requirements = ToDistinctList(input.Requirements);
        target.Assumptions = ToDistinctList(input.Assumptions);
        target.Constraints = ToDistinctList(input.Constraints);
        target.Examples = ToDistinctList(input.Examples);
        target.Strategy = ToDistinctList(input.Strategy);
        target.Validation = ToDistinctList(input.Evaluation);
    }

    internal static void UpdatePersonaEntity(PersonaData input, PersonaEntity target) {
        target.Name = input.Name;
        target.Description = input.Description;
        UpdateCharacteristicsEntity(input.Characteristics, target.Characteristics);
        target.Facts = ToDistinctList(input.Facts);
        UpdateToolEntityList(input.KnownTools, target.Tools);
    }

    internal static void UpdateCharacteristicsEntity(CharacteristicsData input, CharacteristicsEntity target) {
        target.Cognition = ToDistinctList(input.Cognition);
        target.Disposition = ToDistinctList(input.Disposition);
        target.Interaction = ToDistinctList(input.Interaction);
        target.Attitude = ToDistinctList(input.Attitude);
    }

    private static void UpdateToolEntityList(IEnumerable<ToolData> inputList, ICollection<ToolEntity> targetList) {
        targetList.Clear();
        foreach (var input in inputList)
            targetList.Add(ToToolEntity(input));
    }

    internal static void UpdateToolEntity(ToolData input, ToolEntity target) {
        target.Name = input.Name;
        target.Description = input.Description;
        UpdateArgumentEntityList(input.Arguments.AsIndexed(), target.Arguments);
    }

    private static void UpdateArgumentEntityList(IEnumerable<Indexed<ArgumentData>> inputList, ICollection<ArgumentEntity> targetList) {
        targetList.Clear();
        foreach (var input in inputList)
            targetList.Add(ToArgumentEntity(input));
    }

    internal static void UpdateArgumentEntity(Indexed<ArgumentData> input, ArgumentEntity target) {
        target.Name = input.Value.Name;
        target.Description = input.Value.Description;
        target.Type = input.Value.Type;
        target.Choices = input.Value.Type != ArgumentType.Enum ? [] : ToDistinctList(input.Value.Choices);
    }
    #endregion
    #region Create New Entity
    internal static User ToUser(UserData input) {
        var entity = new User();
        UpdateUser(input, entity);
        return entity;
    }

    internal static WorldEntity ToWorldEntity(WorldData input) {
        var entity = new WorldEntity();
        UpdateWorldEntity(input, entity);
        return entity;
    }

    public static ChatEntity ToChatEntity(ChatData input) {
        var entity = new ChatEntity();
        UpdateChatEntity(input, entity);
        return entity;
    }

    internal static ChatAgentEntity ToChatAgentEntity(ChatAgentData input) {
        var entity = new ChatAgentEntity();
        UpdateChatAgentEntity(input, entity);
        return entity;
    }

    internal static ChatAgentOptionsEntity ToChatAgentOptionsEntity(ChatAgentOptionsData input) {
        var entity = new ChatAgentOptionsEntity();
        UpdateChatAgentOptionsEntity(input, entity);
        return entity;
    }

    internal static MessageEntity ToMessageEntity(MessageData input) {
        var entity = new MessageEntity();
        UpdateMessageEntity(input, entity);
        return entity;
    }

    internal static ProviderEntity ToProviderEntity(ProviderData input) {
        var entity = new ProviderEntity();
        UpdateProviderEntity(input, entity);
        return entity;
    }

    internal static ModelEntity ToModelEntity(ModelData input) {
        var entity = new ModelEntity();
        UpdateModelEntity(input, entity);
        return entity;
    }

    internal static ToolEntity ToToolEntity(ToolData input) {
        var entity = new ToolEntity();
        UpdateToolEntity(input, entity);
        return entity;
    }

    internal static ArgumentEntity ToArgumentEntity(Indexed<ArgumentData> input) {
        var entity = new ArgumentEntity();
        UpdateArgumentEntity(input, entity);
        return entity;
    }

    public static InstructionsEntity ToInstructionsEntity(InstructionsData input) {
        var entity = new InstructionsEntity();
        UpdateInstructionsEntity(input, entity);
        return entity;
    }

    internal static PersonaEntity ToPersonaEntity(PersonaData input) {
        var entity = new PersonaEntity();
        UpdatePersonaEntity(input, entity);
        return entity;
    }

    #endregion

    private static List<string> ToDistinctList(IEnumerable<string> items)
        => items.Where(p => !string.IsNullOrWhiteSpace(p))
                .Distinct()
                .ToList();
}
