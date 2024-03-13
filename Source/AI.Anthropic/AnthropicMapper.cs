namespace DotNetToolbox.AI.Anthropic;

public class AnthropicMapper<TAgent>(TAgent agent)
    where TAgent : IAgent<AnthropicAgentOptions> {

    public AnthropicChatRequest CreateRequest(IChat chat)
        => new() {
            Model = agent.Options.Model,
            Temperature = agent.Options.Temperature,
            MaximumOutputTokens = agent.Options.MaximumOutputTokens,
            StopSequences = agent.Options.StopSequences.Count == 0 ? null : [.. agent.Options.StopSequences],
            MinimumTokenProbability = agent.Options.TokenProbabilityCutOff,
            UseStreaming = agent.Options.UseStreaming,

            Messages = chat.Messages.ToArray(o => new AnthropicRequestMessage(o)),

            MaximumTokenSamples = agent.Options.MaximumTokensToSample,
            System = CreateSystemMessage(chat),
        };

    private string CreateSystemMessage(IChat chat) {
        var builder = new StringBuilder();
        builder.AppendLine(agent.World.ToString());
        builder.AppendLine(agent.Persona.Profile.ToString());
        builder.AppendLine(agent.Persona.Skills.ToString());
        builder.AppendLine(chat.Instructions.ToString());
        return builder.ToString();
    }

    public Message CreateResponseMessage(IChat chat, AnthropicChatResponse response) {
        chat.TotalTokens += (uint)(response.Usage.InputTokens + response.Usage.OutputTokens);
        return new("assistant", response.Completion.ToArray(i => new MessagePart(i.Type, ((object?)i.Text ?? i.Image)!)));
    }
}
