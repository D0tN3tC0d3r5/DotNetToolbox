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

    protected override Message ExtractMessage(IChat chat, JobContext context, ChatResponse response) {
        chat.InputTokens += (uint)response.Usage.InputTokens;
        chat.OutputTokens += (uint)response.Usage.OutputTokens;
        var content = response.Content.ToArray(ToMessagePart);
        var message = new Message(MessageRole.Assistant);
        message.AddRange(content);
        message.IsPartial = response.StopReason == "max_tokens";
        return message;
    }

    private static MessagePart ToMessagePart(MessageContent content)
        => new(ToMessageContentType(content.Type), (object?)content.Text ?? content.Image);

    private static MessagePartContentType ToMessageContentType(string type) => type switch {
        "text" => MessagePartContentType.Text,
        "image" => MessagePartContentType.Image,
        "tool_call" => MessagePartContentType.ToolCall,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown message part content type."),
    };
};
