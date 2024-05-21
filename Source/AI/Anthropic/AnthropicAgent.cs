namespace DotNetToolbox.AI.Anthropic;

public class AnthropicAgent([FromKeyedServices("Anthropic")]IHttpClientProviderFactory factory, ILogger<AnthropicAgent> logger)
    : Agent<AnthropicAgent, ChatRequest, ChatResponse>("Anthropic", factory, logger) {
    protected override ChatRequest CreateRequest(IChat chat, World world, UserProfile userProfile, IAgent agent)
        => new() {
            Model = agent.AgentModel.ModelId,
            Temperature = agent.AgentModel.Temperature,
            MaximumOutputTokens = agent.AgentModel.MaximumOutputTokens,
            StopSequences = agent.AgentModel.StopSequences.Count == 0 ? null : [.. agent.AgentModel.StopSequences],
            MinimumTokenProbability = agent.AgentModel.TokenProbabilityCutOff,
            ResponseIsStream = agent.AgentModel.ResponseIsStream,
            Messages = chat.Messages.ToArray(o => new RequestMessage(o)),
            System = CreateSystemMessage(chat, world, userProfile, agent),
        };

    protected override Message GetResponseMessage(IChat chat, ChatResponse response) {
        chat.TotalTokens += (uint)(response.Usage.InputTokens + response.Usage.OutputTokens);
        return new("assistant", response.Content.ToArray(i => new MessagePart(i.Type, ((object?)i.Text ?? i.Image)!)));
    }
};
