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
            Tools = chat.Options.Tools.Count == 0 ? null : [.. chat.Options.Tools],
            Messages = chat.Messages.ToArray(o => new OpenAIRequestMessage(o)),
        };

    protected override void UpdateUsage(OpenAIChat chat, OpenAIChatResponse response)
        => chat.TotalNumberOfTokens = response.Usage?.TotalTokens ?? chat.TotalNumberOfTokens;

    protected override Message CreateOutput(OpenAIChatResponse source)
        => source.Choices[0].Message.Content switch {
            string txt => new("assistant", [new("text", txt)]),
            OpenAIToolCall[] tcs => new("assistant", [new("tool_calls", tcs)]),
            _ => throw new NotSupportedException(),
        };
}
