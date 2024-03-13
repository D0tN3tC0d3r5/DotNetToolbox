namespace DotNetToolbox.AI.OpenAI;

public class OpenAIMapper<TAgent>(TAgent agent)
    where TAgent : IAgent<OpenAIAgentOptions> {

    public OpenAIChatRequest CreateRequest(IChat chat) {
        var system = new OpenAIChatRequestMessage(CreateSystemMessage(chat));
        return new() {
            Model = agent.Options.Model,
            Temperature = agent.Options.Temperature,
            MaximumOutputTokens = agent.Options.MaximumOutputTokens,
            StopSequences = agent.Options.StopSequences.Count == 0 ? null : [.. agent.Options.StopSequences],
            MinimumTokenProbability = agent.Options.TokenProbabilityCutOff,
            UseStreaming = agent.Options.UseStreaming,

            Messages = [system, .. chat.Messages.ToArray(o => new OpenAIChatRequestMessage(o))],

            NumberOfChoices = agent.Options.NumberOfChoices,
            FrequencyPenalty = agent.Options.FrequencyPenalty,
            PresencePenalty = agent.Options.PresencePenalty,
            Tools = agent.Persona.Skills.Count == 0 ? null : agent.Persona.Skills.ToArray(ToRequestToolCall),
        };
    }

    private string CreateSystemMessage(IChat chat) {
        var builder = new StringBuilder();
        builder.AppendLine(agent.World.ToString());
        builder.AppendLine(agent.Persona.Profile.ToString());
        builder.AppendLine(agent.Persona.Skills.ToString());
        builder.AppendLine(chat.Instructions.ToString());
        return builder.ToString();
    }

    public Message CreateResponseMessage(IChat chat, OpenAIChatResponse response) {
        chat.TotalTokens = (uint)(response.Usage?.TotalTokens ?? (int)chat.TotalTokens);
        return response.Choices[0].Message.Content switch {
            OpenAIChatResponseToolRequest[] tcs => new("assistant", [new MessagePart("tool_calls", tcs)]),
            _ => new("assistant", [new MessagePart("text", response.Choices[0].Message.Content.ToString()!)]),
        };
    }

    private static OpenAIChatRequestTool ToRequestToolCall(Skill skill)
        => new("function", new(skill.Name, CreateParameterList(skill), skill.Description));

    private static OpenAIChatRequestToolFunctionCallParameters? CreateParameterList(Skill skill) {
        var parameters = GetParameters(skill);
        var required = GetRequiredParameters(skill);
        return parameters is null && required is null ? null : new(parameters, required);
    }

    private static Dictionary<string, OpenAIChatRequestToolFunctionCallParameter>? GetParameters(Skill skill) {
        var result = skill.Arguments.ToDictionary<Argument, string, OpenAIChatRequestToolFunctionCallParameter>(k => k.Name, ToParameter);
        return result.Count == 0 ? null : result;
    }

    private static string[]? GetRequiredParameters(Skill skill) {
        var result = skill.Arguments.Where(p => p.IsRequired).ToArray(p => p.Name);
        return result.Length == 0 ? null : result;
    }

    private static OpenAIChatRequestToolFunctionCallParameter ToParameter(Argument argument)
        => new(argument.Type, argument.Options, argument.Description);
}
