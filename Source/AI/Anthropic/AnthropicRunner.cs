namespace DotNetToolbox.AI.Anthropic;

public class AnthropicRunner
    : AgentRunner<AnthropicAgentOptions, AnthropicChatRequest, AnthropicChatResponse> {
    public AnthropicRunner(AnthropicAgent agent, World world, IHttpClientProvider httpClientProvider)
        : base(agent, world, httpClientProvider) {
    }

    protected override AnthropicChatRequest CreateRequest(RequestPackage package)
        => new() {
            Model = Agent.Options.Model,
            Temperature = Agent.Options.Temperature,
            MaximumOutputTokens = Agent.Options.MaximumOutputTokens,
            StopSequences = Agent.Options.StopSequences.Count == 0 ? null : [.. Agent.Options.StopSequences],
            MinimumTokenProbability = Agent.Options.MinimumTokenProbability,
            UseStreaming = Agent.Options.UseStreaming,
            System = CreateSystemMessage(),
            Messages = package.Chat.Messages.ToArray(o => new AnthropicRequestMessage(o)),
        };

    private string CreateSystemMessage() => "You are a helpful agent.";

    protected override Message CreateResponseMessage(IChat chat, AnthropicChatResponse response) {
        chat.TotalTokens += (uint)(response.Usage.InputTokens + response.Usage.OutputTokens);
        return new("assistant", response.Completion.ToArray(i => new MessagePart(i.Type, ((object?)i.Text ?? i.Image)!)));
    }
}
