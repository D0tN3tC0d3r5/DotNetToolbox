namespace DotNetToolbox.AI.OpenAI;

public class OpenAIAgent([FromKeyedServices("OpenAI")] IHttpClientProviderFactory factory, ILogger<OpenAIAgent> logger)
    : Agent<OpenAIAgent, ChatRequest, ChatResponse>("OpenAI", factory, logger) {
    protected override ChatRequest CreateRequest(IJob job, IChat chat)
        => new(World, job, this, chat) {
            Temperature = Model.Temperature,
            StopSequences = Model.StopSequences.Count == 0 ? null : [.. Model.StopSequences],
            MinimumTokenProbability = Model.TokenProbabilityCutOff,
            ResponseIsStream = Model.ResponseIsStream,

            NumberOfChoices = 1,
            FrequencyPenalty = 0,
            PresencePenalty = 0,
            Tools = Persona.KnownTools.Count == 0 ? null : Persona.KnownTools.ToArray(ToRequestToolCall),
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
