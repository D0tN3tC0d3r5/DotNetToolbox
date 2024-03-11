namespace DotNetToolbox.AI.OpenAI;

public class OpenAIRunner(IAgent agent,
                          World world,
                          IHttpClientProvider httpClientProvider,
                          ILogger<OpenAIRunner> logger)
    : AgentRunner<OpenAIRunner, OpenAIAgentOptions, OpenAIChatRequest, OpenAIChatResponse>(agent, world, httpClientProvider, logger) {

    public OpenAIRunner(World world,
                        IHttpClientProvider httpClientProvider,
                        ILogger<OpenAIRunner> logger)
        : this(new OpenAIAgent(), world, httpClientProvider, logger) {
    }

    public OpenAIRunner(IAgent agent,
                        IEnvironment environment,
                        IHttpClientProvider httpClientProvider,
                        ILogger<OpenAIRunner> logger)
        : this(agent, new World(environment), httpClientProvider, logger) {
    }

    public OpenAIRunner(IEnvironment environment,
                        IHttpClientProvider httpClientProvider,
                        ILogger<OpenAIRunner> logger)
        : this(new World(environment), httpClientProvider, logger) {
    }

    protected override OpenAIChatRequest CreateRequest(RequestPackage package) {
        var system = new OpenAIChatRequestMessage(CreateSystemMessage());
        var options = (OpenAIAgentOptions)Agent.Options;
        return new() {
            Model = options.Model,
            Temperature = options.Temperature,
            MaximumOutputTokens = options.MaximumOutputTokens,
            StopSequences = (options.StopSequences?.Count ?? 0) == 0 ? null : [.. options.StopSequences!],
            MinimumTokenProbability = options.TokenProbabilityCutOff,
            UseStreaming = options.UseStreaming,
            Messages = [system, .. package.Chat.Messages?.ToArray(o => new OpenAIChatRequestMessage(o)) ?? []],

            NumberOfChoices = options.NumberOfChoices,
            FrequencyPenalty = options.FrequencyPenalty,
            PresencePenalty = options.PresencePenalty,
            Tools = (Agent.Skills?.Count ?? 0) == 0 ? null : Agent.Skills!.ToArray(ToRequestToolCall),
        };
    }

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
