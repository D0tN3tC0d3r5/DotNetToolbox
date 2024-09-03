
namespace DotNetToolbox.AI.Anthropic;

public class AnthropicAgent([FromKeyedServices("Anthropic")] IHttpClientProviderFactory factory, ILogger<AnthropicAgent> logger)
    : Agent<AnthropicAgent, ChatRequest, ChatResponse>("Anthropic", factory, logger) {
    protected override ChatRequest CreateRequest(IJob job, IChat chat)
        => new(this, chat) {
            Temperature = Settings.Temperature,
            StopSequences = Settings.StopSequences.Count == 0 ? null : [.. Settings.StopSequences],
            MinimumTokenProbability = Settings.TokenProbabilityCutOff,
            ResponseIsStream = Settings.ResponseIsStream,
            MaximumTokenSamples = 0,
        };

    protected override bool UpdateChat(IChat chat, ChatResponse response) {
        chat.TotalTokens += (uint)(response.Usage.InputTokens + response.Usage.OutputTokens);
        var message = ToMessage(response);
        chat.Messages.Add(message);
        return message.IsComplete;
    }

    private static Message ToMessage(ChatResponse response) {
        var parts = response.Content.ToArray(ToMessagePart);
        return new Message(MessageRole.Assistant, parts) {
            IsComplete = response.StopReason != "max_tokens"
        };
    }

    private static MessagePart ToMessagePart(MessageContent i)
        => new(ToContentType(i.Type), ((object?)i.Text ?? i.Image)!);

    private static MessagePartContentType ToContentType(string type) => type switch {
        "text" => MessagePartContentType.Text,
        "image" => MessagePartContentType.Image,
        "tool_call" => MessagePartContentType.ToolCall,
        _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown message part content type."),
    };
};
