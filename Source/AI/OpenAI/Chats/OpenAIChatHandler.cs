namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIChatHandler(IHttpClientProvider httpClientProvider, ILogger<OpenAIChatHandler> logger)
    : ChatHandler<OpenAIChatHandler, OpenAIChat, OpenAIChatOptions, OpenAIChatRequest, OpenAIChatResponse>(httpClientProvider, logger) {
    protected override OpenAIChatRequest CreateRequest(OpenAIChat chat)
        => new() {
            Model = chat.Options.Model,
            Temperature = chat.Options.Temperature,
            MaximumTokensPerMessage = (int?)chat.Options.MaximumTokensPerMessage,
            FrequencyPenalty = chat.Options.FrequencyPenalty,
            PresencePenalty = chat.Options.PresencePenalty,
            NumberOfChoices = chat.Options.NumberOfChoices,
            StopSequences = chat.Options.StopSequences.Count == 0 ? null : [.. chat.Options.StopSequences],
            MinimumTokenProbability = chat.Options.MinimumTokenProbability,
            UseStreaming = chat.Options.UseStreaming,
            Tools = chat.Options.Tools.Count == 0 ? null : chat.Options.Tools.ToArray(ToRequestToolCall),
            Messages = chat.Messages.ToArray(o => new OpenAIChatRequestMessage(o) { Name = chat.Options.AgentName }),
        };

    protected override void UpdateUsage(OpenAIChat chat, OpenAIChatResponse response)
        => chat.TotalNumberOfTokens = response.Usage?.TotalTokens ?? chat.TotalNumberOfTokens;

    protected override Message CreateOutput(OpenAIChatResponse source)
        => source.Choices[0].Message.Content switch {
            OpenAIChatResponseToolCall[] tcs => new("assistant", [new("tool_calls", tcs)]),
            _ => new("assistant", [new("text", source.Choices[0].Message.Content.ToString()!)]),
        };

    private static OpenAIChatRequestToolCall ToRequestToolCall(OpenAIChatTool toolCall)
        => new(toolCall.Id, new(toolCall.Name, CreateParameterList(toolCall), toolCall.Description));
    private static OpenAIChatRequestToolCallFunctionParameters? CreateParameterList(OpenAIChatTool toolCall) {
        var parameters = GetParameters(toolCall);
        var required = GetRequiredParameters(toolCall);
        return parameters is null && required is null ? null : new(parameters, required);
    }

    private static Dictionary<string, OpenAIChatRequestToolCallFunctionParameter>? GetParameters(OpenAIChatTool toolCall) {
        var result = toolCall.Parameters.ToDictionary(k => k.Key, ToParameter);
        return result.Count == 0 ? null : result;
    }

    private static string[]? GetRequiredParameters(OpenAIChatTool toolCall) {
        var result = toolCall.Parameters.Where(p => p.Value.IsRequired).ToArray(p => p.Key);
        return result.Length == 0 ? null : result;
    }

    private static OpenAIChatRequestToolCallFunctionParameter ToParameter(KeyValuePair<string, OpenAIChatToolParameter> parameter)
        => new(parameter.Value.Type, parameter.Value.Options, parameter.Value.Description);
}
