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

    protected override bool ProcessResponse(IChat chat, ChatResponse response, Message resultMessage) {
        chat.InputTokens += (uint?)(response.Usage?.PromptTokens) ?? 0;
        chat.OutputTokens += (uint?)(response.Usage?.CompletionTokens) ?? 0;
        var hasFinished = response.Choices[0].StopReason != "length";
        var content = response.Choices[0].Message.Content;
        var message = content switch {
            ChatResponseToolRequest[] tcs => new MessagePart(MessagePartContentType.ToolCall, tcs),
            _ => new MessagePart(content as string ?? string.Empty),
        };
        resultMessage.AddRange([message]);
        chat.AppendMessage(hasFinished ? MessageRole.Assistant : MessageRole.User, message);
        return hasFinished;
    }
}
