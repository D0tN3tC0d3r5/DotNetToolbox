namespace DotNetToolbox.AI.OpenAI;

public class OpenAIHttpConnection([FromKeyedServices("OpenAI")] IHttpClientProviderAccessor factory, ILogger<OpenAIHttpConnection> logger)
    : HttpConnection<OpenAIHttpConnection, ChatRequest, ChatResponse>("OpenAI", factory, logger) {
    protected override ChatRequest CreateRequest(IJob job, IChat chat)
        => new(this, job.Context.Model, chat) {
            Temperature = Settings.Temperature,
            StopSequences = Settings.StopSequences.Count == 0 ? null : [.. Settings.StopSequences],
            MinimumTokenProbability = Settings.TokenProbabilityCutOff,
            ResponseIsStream = Settings.ResponseIsStream,

            NumberOfChoices = 1,
            FrequencyPenalty = 0,
            PresencePenalty = 0,
            Tools = Tools.Count == 0 ? null : Tools.ToArray(ToRequestToolCall),
            ForceToolCall = null,
            ResponseFormat = null,
        };
    protected override bool UpdateChat(IChat chat, ChatResponse response) {
        chat.TotalTokens = (uint)(response.Usage?.TotalTokens ?? (int)chat.TotalTokens);
        var message = ToMessage(response.Choices[0]);
        chat.Messages.Add(message);
        return message.IsComplete;
    }

    private static Message ToMessage(ChatResponseChoice firstChoice) {
        var content = firstChoice.Message.Content;
        var message = content switch {
            ChatResponseToolRequest[] tcs => new Message(MessageRole.Assistant, new MessagePart(MessagePartContentType.ToolCall, tcs)) {
                IsComplete = true,
            },
            _ => new Message(MessageRole.Assistant, $"{content}") {
                IsComplete = firstChoice.StopReason != "length",
            },
        };
        return message;
    }

    private static ChatRequestTool ToRequestToolCall(Tool tool)
        => new("function", new(tool.Name, CreateParameterList(tool), tool.Description));

    private static ChatRequestToolFunctionCallParameters? CreateParameterList(Tool tool) {
        var parameters = GetParameters(tool);
        var required = GetRequiredParameters(tool);
        return parameters is null && required is null ? null : new(parameters, required);
    }

    private static Dictionary<string, ChatRequestToolFunctionCallParameter>? GetParameters(Tool tool) {
        var result = tool.Arguments.ToDictionary<ToolArgument, string, ChatRequestToolFunctionCallParameter>(k => k.Name, ToParameter);
        return result.Count == 0 ? null : result;
    }

    private static string[]? GetRequiredParameters(Tool tool) {
        var result = tool.Arguments.Where(p => p.IsRequired).ToArray(p => p.Name);
        return result.Length == 0 ? null : result;
    }

    private static ChatRequestToolFunctionCallParameter ToParameter(ToolArgument argument)
        => new(argument.Type.ToString(), argument.Options, argument.Description);
}
