namespace DotNetToolbox.AI.OpenAI;

public class Mapper()
    : IMapper {
    IChatRequest IMapper.CreateRequest(IStandardAgent agent, IChat chat) => CreateRequest((IStandardAgent<AgentOptions>)agent, chat);
    public static ChatRequest CreateRequest(IStandardAgent<AgentOptions> agent, IChat chat) {
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
            Tools = agent.Persona.KnownTools.Count == 0 ? null : agent.Persona.KnownTools.ToArray(ToRequestToolCall),
        };
    }

    private static string CreateSystemMessage(IStandardAgent agent, IChat chat) {
        var builder = new StringBuilder();
        builder.AppendLine(agent.World.ToString());
        builder.AppendLine(agent.Persona.ToString());
        builder.AppendLine(chat.Instructions.ToString());
        return builder.ToString();
    }

    Message IMapper.CreateResponseMessage(IChat chat, IChatResponse response)
        => CreateResponseMessage(chat, (ChatResponse)response);
    public static Message CreateResponseMessage(IChat chat, ChatResponse response) {
        chat.TotalTokens = (uint)(response.Usage?.TotalTokens ?? (int)chat.TotalTokens);
        return response.Choices[0].Message.Content switch {
            ChatResponseToolRequest[] tcs => new("assistant", [new("tool_calls", tcs)]),
            _ => new("assistant", [new("text", response.Choices[0].Message.Content.ToString()!)]),
        };
    }

    private static ChatRequestTool ToRequestToolCall(Tool tool)
        => new("function", new(tool.Name, CreateParameterList(tool), tool.Description));

    private static ChatRequestToolFunctionCallParameters? CreateParameterList(Tool tool) {
        var parameters = GetParameters(tool);
        var required = GetRequiredParameters(tool);
        return parameters is null && required is null ? null : new(parameters, required);
    }

    private static Dictionary<string, ChatRequestToolFunctionCallParameter>? GetParameters(Tool tool) {
        var result = tool.Arguments.ToDictionary<Argument, string, ChatRequestToolFunctionCallParameter>(k => k.Name, ToParameter);
        return result.Count == 0 ? null : result;
    }

    private static string[]? GetRequiredParameters(Tool tool) {
        var result = tool.Arguments.Where(p => p.IsRequired).ToArray(p => p.Name);
        return result.Length == 0 ? null : result;
    }

    private static ChatRequestToolFunctionCallParameter ToParameter(Argument argument)
        => new(argument.Type.ToString(), argument.Options, argument.Description);
}
