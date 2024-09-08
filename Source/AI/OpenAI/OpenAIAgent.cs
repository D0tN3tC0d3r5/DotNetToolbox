namespace DotNetToolbox.AI.OpenAI;

public class OpenAIAgent(IServiceProvider services, ILogger<OpenAIAgent> logger)
    : Agent<OpenAIAgent, ChatRequest, ChatResponse>("OpenAI", services, logger) {
    protected override ChatRequest CreateRequest(IChat chat, JobContext context)
        => new(this, context.Model, chat) {
            Temperature = Settings.Temperature,
            StopSequences = Settings.StopSequences.Count == 0 ? null : [.. Settings.StopSequences],
            MinimumTokenProbability = Settings.TokenProbabilityCutOff,
            ResponseIsStream = Settings.ResponseIsStream,

            NumberOfChoices = 1,
            FrequencyPenalty = 0,
            PresencePenalty = 0,
            Tools = [],
            ForceToolCall = null,
            ResponseFormat = null,
        };

    protected override Message ExtractMessage(IChat chat, JobContext context, ChatResponse response) {
        chat.InputTokens += (uint?)(response.Usage?.PromptTokens) ?? 0;
        chat.OutputTokens += (uint?)(response.Usage?.CompletionTokens) ?? 0;
        var content = response.Choices[0].Message.Content;
        var messagePart = content switch {
            ChatResponseToolRequest[] tcs => new(MessagePartContentType.ToolCall, tcs),
            _ => new MessagePart(content as string ?? string.Empty),
        };
        var message = new Message(MessageRole.Assistant);
        message.AddRange([messagePart]);
        message.IsPartial = response.Choices[0].StopReason != "length";
        return message;
    }
}
