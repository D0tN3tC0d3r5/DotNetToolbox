using DotNetToolbox.AI.Agents;

namespace DotNetToolbox.AI.Anthropic.Chats;

public class AnthropicChatHandler
    : ChatHandler<AnthropicChatHandler, AnthropicChatOptions, AnthropicChatRequest, AnthropicChatResponse> {
    public AnthropicChatHandler(World world, IHttpClientProvider httpClientProvider, AnthropicChatOptions options, IChat chat)
        : base(world, options, chat, httpClientProvider) {
    }

    protected override AnthropicChatRequest CreateRequest(IAgent agent) => new() {
        Model = Options.Model,
        Temperature = Options.Temperature,
        MaximumTokensPerMessage = (int)Options.MaximumOutputTokens,
        StopSequences = Options.StopSequences.Count == 0 ? null : [.. Options.StopSequences],
        MinimumTokenProbability = Options.MinimumTokenProbability,
        UseStreaming = Options.UseStreaming,
        System = CreateSystemMessage(World, agent),
        Messages = Chat.Messages.ToArray(o => new AnthropicRequestMessage(o)),
    };
    protected override string CreateSystemMessage(World world, IAgent agent) => "You are a helpful agent.";

    protected override Message CreateMessage(AnthropicChatResponse response) {
        Chat.TotalNumberOfTokens += response.Usage.InputTokens + response.Usage.OutputTokens;
        return new("assistant", response.Completion.ToArray(i => new MessagePart(i.Type, ((object?)i.Text ?? i.Image)!)));
    }
}
