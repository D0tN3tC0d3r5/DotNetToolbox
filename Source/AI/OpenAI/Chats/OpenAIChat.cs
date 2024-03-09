namespace DotNetToolbox.AI.OpenAI.Chats;

public class OpenAIChat
    : Chat<OpenAIChat, OpenAIChatOptions, OpenAIChatRequest, OpenAIChatResponse> {
    public OpenAIChat(IHttpClientProvider httpClientProvider, OpenAIChatOptions? options = null)
        : base(httpClientProvider, options) {
        Messages.Add(new("system", [new("text", Options.SystemMessage)]));
    }

    protected override OpenAIChatRequest CreateRequest()
        => new() {
            Model = Options.Model,
            Temperature = Options.Temperature,
            MaximumTokensPerMessage = (int)Options.MaximumTokensPerMessage,
            FrequencyPenalty = Options.FrequencyPenalty,
            PresencePenalty = Options.PresencePenalty,
            NumberOfChoices = Options.NumberOfChoices,
            StopSequences = Options.StopSequences.Count == 0 ? null : [.. Options.StopSequences],
            MinimumTokenProbability = Options.MinimumTokenProbability,
            UseStreaming = Options.UseStreaming,
            Tools = Options.Tools.Count == 0 ? null : Options.Tools.ToArray(ToRequestToolCall),
            Messages = Messages.ToArray(o => new OpenAIChatRequestMessage(o) { Name = Options.AgentName }),
        };

    protected override Message CreateMessage(OpenAIChatResponse response) {
        TotalNumberOfTokens = response.Usage?.TotalTokens ?? TotalNumberOfTokens;
        return response.Choices[0].Message.Content switch {
            OpenAIChatResponseToolCall[] tcs => new("assistant", [new("tool_calls", tcs)]),
            _ => new("assistant", [new("text", response.Choices[0].Message.Content.ToString()!)]),
        };
    }

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
