namespace DotNetToolbox.AI.Anthropic.Chats;

public class AnthropicChatHandler(IHttpClientProvider httpClientProvider, ILogger<AnthropicChatHandler> logger)
    : ChatHandler<AnthropicChatHandler, AnthropicChat, AnthropicChatOptions, AnthropicChatRequest, AnthropicChatResponse>(httpClientProvider, logger) {
    protected override AnthropicChatRequest CreateRequest(AnthropicChat chat) => new() {
        Model = chat.Options.Model,
        Temperature = chat.Options.Temperature,
        MaximumTokensPerMessage = (int)chat.Options.MaximumTokensPerMessage,
        StopSequences = chat.Options.StopSequences.Count == 0 ? null : [.. chat.Options.StopSequences],
        MinimumTokenProbability = chat.Options.MinimumTokenProbability,
        UseStreaming = chat.Options.UseStreaming,
        Messages = chat.Messages.ToArray(o => new AnthropicRequestMessage(o)),
    };

    protected override void UpdateUsage(AnthropicChat chat, AnthropicChatResponse response)
        => chat.TotalNumberOfTokens += response.Usage.InputTokens + response.Usage.OutputTokens;

    protected override Message CreateOutput(AnthropicChatResponse source)
        => new("assistant", source.Completion.ToArray(i => new Content(i.Type, ((object?)i.Text ?? i.Image)!)));
}
