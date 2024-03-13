namespace DotNetToolbox.AI.OpenAI;

internal class Mapper : IMapper {
    IChatRequest IMapper.CreateRequest(IAgent agent, IChat chat) => CreateRequest((IAgent<AgentOptions>)agent, chat);
    public static ChatRequest CreateRequest(IAgent<AgentOptions> agent, IChat chat) {
        var system = new ChatRequestMessage(CreateSystemMessage(agent, chat));
        return new() {
            Model = agent.Options.Model,
            Temperature = agent.Options.Temperature,
            MaximumOutputTokens = agent.Options.MaximumOutputTokens,
            StopSequences = agent.Options.StopSequences.Count == 0 ? null : [.. agent.Options.StopSequences],
            MinimumTokenProbability = agent.Options.TokenProbabilityCutOff,
            UseStreaming = agent.Options.UseStreaming,

            Messages = [system, .. chat.Messages.ToArray(o => new ChatRequestMessage(o))],

            NumberOfChoices = agent.Options.NumberOfChoices,
            FrequencyPenalty = agent.Options.FrequencyPenalty,
            PresencePenalty = agent.Options.PresencePenalty,
            Tools = agent.Persona.Skills.Count == 0 ? null : agent.Persona.Skills.ToArray(ToRequestToolCall),
        };
    }

    private static string CreateSystemMessage(IAgent agent, IChat chat) {
        var builder = new StringBuilder();
        builder.AppendLine(agent.World.ToString());
        builder.AppendLine(agent.Persona.Profile.ToString());
        builder.AppendLine(agent.Persona.Skills.ToString());
        builder.AppendLine(chat.Instructions.ToString());
        return builder.ToString();
    }

    Message IMapper.CreateResponseMessage(IChat chat, IChatResponse response)
        => CreateResponseMessage(chat, (ChatResponse)response);
    public static Message CreateResponseMessage(IChat chat, ChatResponse response) {
        chat.TotalTokens = (uint)(response.Usage?.TotalTokens ?? (int)chat.TotalTokens);
        return response.Choices[0].Message.Content switch {
            ChatResponseToolRequest[] tcs => new("assistant", [new MessagePart("tool_calls", tcs)]),
            _ => new("assistant", [new MessagePart("text", response.Choices[0].Message.Content.ToString()!)]),
        };
    }

    private static ChatRequestTool ToRequestToolCall(Skill skill)
        => new("function", new(skill.Name, CreateParameterList(skill), skill.Description));

    private static ChatRequestToolFunctionCallParameters? CreateParameterList(Skill skill) {
        var parameters = GetParameters(skill);
        var required = GetRequiredParameters(skill);
        return parameters is null && required is null ? null : new(parameters, required);
    }

    private static Dictionary<string, ChatRequestToolFunctionCallParameter>? GetParameters(Skill skill) {
        var result = skill.Arguments.ToDictionary<Argument, string, ChatRequestToolFunctionCallParameter>(k => k.Name, ToParameter);
        return result.Count == 0 ? null : result;
    }

    private static string[]? GetRequiredParameters(Skill skill) {
        var result = skill.Arguments.Where(p => p.IsRequired).ToArray(p => p.Name);
        return result.Length == 0 ? null : result;
    }

    private static ChatRequestToolFunctionCallParameter ToParameter(Argument argument)
        => new(argument.Type, argument.Options, argument.Description);
}
