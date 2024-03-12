using DotNetToolbox.AI.Actors;

namespace DotNetToolbox.AI.OpenAI;

public class OpenAIQueuedActor(IAgent agent,
                          World world,
                          IHttpClientProvider httpClientProvider,
                          ILogger<OpenAIQueuedActor> logger)
    : QueuedActor<OpenAIQueuedActor, OpenAIAgentOptions, OpenAIChatRequest, OpenAIChatResponse>(agent, world, httpClientProvider, logger) {

    public OpenAIQueuedActor(World world,
                        IHttpClientProvider httpClientProvider,
                        ILogger<OpenAIQueuedActor> logger)
        : this(new OpenAIAgent(), world, httpClientProvider, logger) {
    }

    public OpenAIQueuedActor(IAgent agent,
                        IEnvironment environment,
                        IHttpClientProvider httpClientProvider,
                        ILogger<OpenAIQueuedActor> logger)
        : this(agent, new World(environment), httpClientProvider, logger) {
    }

    public OpenAIQueuedActor(IEnvironment environment,
                        IHttpClientProvider httpClientProvider,
                        ILogger<OpenAIQueuedActor> logger)
        : this(new World(environment), httpClientProvider, logger) {
    }

    protected override OpenAIChatRequest CreateRequest(IRequestSource source, IChat chat) {
        var system = new OpenAIChatRequestMessage(CreateSystemMessage(chat));
        var options = (OpenAIAgentOptions)Agent.Options;
        return new() {
            Model = options.Model,
            Temperature = options.Temperature,
            MaximumOutputTokens = options.MaximumOutputTokens,
            StopSequences = options.StopSequences.Count == 0 ? null : [.. options.StopSequences],
            MinimumTokenProbability = options.TokenProbabilityCutOff,
            UseStreaming = options.UseStreaming,
            Messages = [system, .. chat.Messages.ToArray(o => new OpenAIChatRequestMessage(o))],

            NumberOfChoices = options.NumberOfChoices,
            FrequencyPenalty = options.FrequencyPenalty,
            PresencePenalty = options.PresencePenalty,
            Tools = Agent.Skills.Count == 0 ? null : Agent.Skills.ToArray(ToRequestToolCall),
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
