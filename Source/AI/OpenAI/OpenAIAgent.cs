using DotNetToolbox.AI.OpenAI.Chats;

namespace DotNetToolbox.AI.OpenAI;

public class OpenAIAgent([FromKeyedServices("OpenAI")] IHttpClientProviderFactory factory, ILogger<OpenAIAgent> logger)
    : Agent<OpenAIAgent, ChatRequest, ChatResponse>("OpenAI", factory, logger) {
    protected override ChatRequest CreateRequest(IChat chat, World world, UserProfile userProfile, IAgent agent) {
        var system = new ChatRequestMessage(CreateSystemMessage(chat, world, userProfile, agent));
        return new() {
            Model = agent.AgentModel.ModelId,
            Temperature = agent.AgentModel.Temperature,
            MaximumOutputTokens = agent.AgentModel.MaximumOutputTokens,
            StopSequences = agent.AgentModel.StopSequences.Count == 0 ? null : [.. agent.AgentModel.StopSequences],
            MinimumTokenProbability = agent.AgentModel.TokenProbabilityCutOff,
            ResponseIsStream = agent.AgentModel.ResponseIsStream,
            Messages = [system, .. chat.Messages.ToArray(o => new ChatRequestMessage(o))],

            NumberOfChoices = 1,
            FrequencyPenalty = 0,
            PresencePenalty = 0,
            Tools = agent.Persona.KnownTools.Count == 0 ? null : agent.Persona.KnownTools.ToArray(ToRequestToolCall),
        };
    }
    protected override Message GetResponseMessage(IChat chat, ChatResponse response) {
        chat.TotalTokens = (uint)(response.Usage?.TotalTokens ?? (int)chat.TotalTokens);
        return response.Choices[0].Message.Content switch {
            ChatResponseToolRequest[] tcs => new("assistant", new MessagePart("tool_calls", tcs)),
            _ => new("assistant", $"{response.Choices[0].Message.Content}"),
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
