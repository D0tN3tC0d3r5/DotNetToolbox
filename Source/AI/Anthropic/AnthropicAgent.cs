using DotNetToolbox.AI.Anthropic.Chats;

namespace DotNetToolbox.AI.Anthropic;

public class AnthropicAgent([FromKeyedServices("Anthropic")] IHttpClientProviderFactory factory, ILogger<AnthropicAgent> logger)
    : Agent<AnthropicAgent, ChatRequest, ChatResponse>("Anthropic", factory, logger) {
    protected override ChatRequest CreateRequest(IChat chat, World world, UserProfile userProfile, IAgent agent)
        => new(chat, world, userProfile, agent) {
            Temperature = agent.AgentModel.Temperature,
            StopSequences = agent.AgentModel.StopSequences.Count == 0 ? null : [.. agent.AgentModel.StopSequences],
            MinimumTokenProbability = agent.AgentModel.TokenProbabilityCutOff,
            ResponseIsStream = agent.AgentModel.ResponseIsStream,
            Messages = chat.Messages.ToArray(o => new ChatRequestMessage(o)),
            MaximumTokenSamples = 0,
        };

    protected override Message GetResponseMessage(IChat chat, ChatResponse response) {
        chat.TotalTokens += (uint)(response.Usage.InputTokens + response.Usage.OutputTokens);
        return new("assistant", response.Content.ToArray(i => new MessagePart(i.Type, ((object?)i.Text ?? i.Image)!)));
    }
};
