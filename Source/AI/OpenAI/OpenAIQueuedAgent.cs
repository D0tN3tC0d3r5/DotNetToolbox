namespace DotNetToolbox.AI.OpenAI;

public class OpenAIQueuedAgent(World world,
                                  OpenAIAgentOptions options,
                                  IPersona persona,
                                  IHttpClientProvider httpClientProvider,
                                  ILogger<OpenAIQueuedAgent> logger)
    : QueuedAgent<
        OpenAIQueuedAgent,
        OpenAIAgentOptions,
        OpenAIChatRequest,
        OpenAIChatResponse>(world,
                            options,
                            persona,
                            httpClientProvider,
                            logger) {

    public OpenAIQueuedAgent(IEnvironment environment,
                             OpenAIAgentOptions options,
                             IPersona persona,
                             IHttpClientProvider httpClientProvider,
                             ILogger<OpenAIQueuedAgent> logger)
        : this(new World(environment), options, persona, httpClientProvider, logger) {
    }

    public OpenAIQueuedAgent(World world,
                             OpenAIAgentOptions options,
                             IHttpClientProvider httpClientProvider,
                             ILogger<OpenAIQueuedAgent> logger)
        : this(world, options, new Persona(), httpClientProvider, logger) {
    }

    public OpenAIQueuedAgent(IEnvironment environment,
                             OpenAIAgentOptions options,
                             IHttpClientProvider httpClientProvider,
                             ILogger<OpenAIQueuedAgent> logger)
        : this(new World(environment), options, new Persona(), httpClientProvider, logger) {
    }

    public OpenAIQueuedAgent(World world,
                             IHttpClientProvider httpClientProvider,
                             ILogger<OpenAIQueuedAgent> logger)
        : this(world, new(), new Persona(), httpClientProvider, logger) {
    }

    public OpenAIQueuedAgent(IEnvironment environment,
                             IHttpClientProvider httpClientProvider,
                             ILogger<OpenAIQueuedAgent> logger)
        : this(new World(environment), new(), new Persona(), httpClientProvider, logger) {
    }

    protected override OpenAIChatRequest CreateRequest(IConsumer source, IChat chat) {
        var system = new OpenAIChatRequestMessage(CreateSystemMessage(chat));
        return new() {
            Model = Options.Model,
            Temperature = Options.Temperature,
            MaximumOutputTokens = Options.MaximumOutputTokens,
            StopSequences = Options.StopSequences.Count == 0 ? null : [.. Options.StopSequences],
            MinimumTokenProbability = Options.TokenProbabilityCutOff,
            UseStreaming = Options.UseStreaming,
            Messages = [system, .. chat.Messages.ToArray(o => new OpenAIChatRequestMessage(o))],

            NumberOfChoices = Options.NumberOfChoices,
            FrequencyPenalty = Options.FrequencyPenalty,
            PresencePenalty = Options.PresencePenalty,
            Tools = Persona.Skills.Count == 0 ? null : Persona.Skills.ToArray(ToRequestToolCall),
        };
    }

    protected override Message CreateResponse(IChat chat, OpenAIChatResponse response) {
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
