namespace DotNetToolbox.AI.Anthropic.Chats;

public class AnthropicChat(IHttpClientProvider httpClientProvider, AnthropicChatOptions? options = null)
    : Chat<AnthropicChat, AnthropicChatOptions, AnthropicChatRequest, AnthropicChatResponse>(httpClientProvider, options) {
    protected override AnthropicChatRequest CreateRequest() => new() {
        Model = Options.Model,
        Temperature = Options.Temperature,
        MaximumTokensPerMessage = (int)Options.MaximumTokensPerMessage,
        StopSequences = Options.StopSequences.Count == 0 ? null : [.. Options.StopSequences],
        MinimumTokenProbability = Options.MinimumTokenProbability,
        UseStreaming = Options.UseStreaming,
        Messages = Messages.ToArray(o => new AnthropicRequestMessage(o)),
    };

    protected override Message CreateMessage(AnthropicChatResponse response) {
        TotalNumberOfTokens += response.Usage.InputTokens + response.Usage.OutputTokens;
        return new("assistant", response.Completion.ToArray(i => new Content(i.Type, ((object?)i.Text ?? i.Image)!)));
    }
}
