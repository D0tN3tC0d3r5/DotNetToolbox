namespace DotNetToolbox.AI.Anthropic;

public class AnthropicAgent(IServiceProvider services, ILogger<AnthropicAgent> logger)
    : Agent<AnthropicAgent, ChatRequest, ChatResponse>("Anthropic", services, logger) {
    protected override ChatRequest CreateRequest(IChat chat, JobContext context)
        => new(this, context.Model, chat) {
            Temperature = Settings.Temperature,
            StopSequences = Settings.StopSequences.Count == 0 ? null : [.. Settings.StopSequences],
            MinimumTokenProbability = Settings.TokenProbabilityCutOff,
            ResponseIsStream = Settings.ResponseIsStream,
            MaximumTokenSamples = 0,
        };

    protected override bool ProcessResponse(IChat chat, ChatResponse response, Message resultMessage) {
        chat.InputTokens += (uint)response.Usage.InputTokens;
        chat.OutputTokens += (uint)response.Usage.OutputTokens;
        var hasFinished = response.StopReason != "max_tokens";
        var content = response.Content.ToArray(ToMessagePart);
        resultMessage.AddRange(content);
        chat.AppendMessage(hasFinished ? MessageRole.Assistant : MessageRole.User, content);
        return hasFinished;
    }

    private static MessagePart ToMessagePart(MessageContent content)
        => new(ToMessageContentType(content.Type), ((object?)content.Text ?? content.Image)!);

    private static MessagePartContentType ToMessageContentType(string type) => type switch {
        "text" => MessagePartContentType.Text,
        "image" => MessagePartContentType.Image,
        "tool_call" => MessagePartContentType.ToolCall,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown message part content type."),
    };
};
