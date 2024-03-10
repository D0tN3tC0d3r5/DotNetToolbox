using DotNetToolbox.AI.Agents;

namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIChatHandler
    : ChatHandler<OpenAIChatHandler, OpenAIChatOptions, OpenAIChatRequest, OpenAIChatResponse> {

    public OpenAIChatHandler(World world, OpenAIChatOptions options, IChat chat, IHttpClientProvider httpClientProvider)
        : base(world, options, chat, httpClientProvider) {
    }

    protected override OpenAIChatRequest CreateRequest(IAgent agent) {
        var system = new OpenAIChatRequestMessage(CreateSystemMessage(World, agent));
        return new() {
            Model = Options.Model,
            Temperature = Options.Temperature,
            MaximumTokensPerMessage = (int)Options.MaximumOutputTokens,
            FrequencyPenalty = Options.FrequencyPenalty,
            PresencePenalty = Options.PresencePenalty,
            NumberOfChoices = Options.NumberOfChoices,
            StopSequences = Options.StopSequences.Count == 0
                                             ? null
                                             : [.. Options.StopSequences],
            MinimumTokenProbability = Options.MinimumTokenProbability,
            UseStreaming = Options.UseStreaming,
            Tools = agent.Skills.Count == 0
                                     ? null
                                     : agent.Skills.ToArray(ToRequestToolCall),
            Messages = [system, .. Chat.Messages.ToArray(o => new OpenAIChatRequestMessage(o))],
        };
    }

    protected override string CreateSystemMessage(World world, IAgent agent) => "You are a helpful agent.";

    protected override Message CreateMessage(OpenAIChatResponse response) {
        Chat.TotalNumberOfTokens = response.Usage?.TotalTokens ?? Chat.TotalNumberOfTokens;
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
