namespace DotNetToolbox.AI.OpenAI;

public class OpenAIRunner
    : AgentRunner<OpenAIAgentOptions, OpenAIChatRequest, OpenAIChatResponse> {

    public OpenAIRunner(OpenAIAgent agent, World world, IHttpClientProvider httpClientProvider)
        : base(agent, world, httpClientProvider) {
    }

    protected override OpenAIChatRequest CreateRequest(RequestPackage package) {
        var system = new OpenAIChatRequestMessage(CreateSystemMessage());
        return new() {
            Model = Agent.Options.Model,
            Temperature = Agent.Options.Temperature,
            MaximumOutputTokens = Agent.Options.MaximumOutputTokens,
            FrequencyPenalty = Agent.Options.FrequencyPenalty,
            PresencePenalty = Agent.Options.PresencePenalty,
            NumberOfChoices = Agent.Options.NumberOfChoices,
            StopSequences = Agent.Options.StopSequences.Count == 0
                                ? null
                                : [.. Agent.Options.StopSequences],
            MinimumTokenProbability = Agent.Options.MinimumTokenProbability,
            UseStreaming = Agent.Options.UseStreaming,
            Tools = Agent.Skills.Count == 0
                        ? null
                        : Agent.Skills.ToArray(ToRequestToolCall),
            Messages = [system, .. package.Chat.Messages.ToArray(o => new OpenAIChatRequestMessage(o))],
        };
    }

    private string CreateSystemMessage() => "You are a helpful agent.";

    protected override Message CreateResponseMessage(IChat chat, OpenAIChatResponse response) {
        chat.TotalTokens = (uint)(response.Usage?.TotalTokens ?? (int)chat.TotalTokens);
        return response.Choices[0].Message.Content switch {
            OpenAIChatResponseToolRequest[] tcs => new("assistant", [new("tool_calls", tcs)]),
            _ => new("assistant", [new("text", response.Choices[0].Message.Content.ToString()!)]),
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
        var result = skill.Arguments.ToDictionary(k => k.Name, ToParameter);
        return result.Count == 0 ? null : result;
    }

    private static string[]? GetRequiredParameters(Skill skill) {
        var result = skill.Arguments.Where(p => p.IsRequired).ToArray(p => p.Name);
        return result.Length == 0 ? null : result;
    }

    private static OpenAIChatRequestToolFunctionCallParameter ToParameter(Argument argument)
        => new(argument.Type, argument.Options, argument.Description);
}
