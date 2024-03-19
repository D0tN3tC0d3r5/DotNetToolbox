namespace DotNetToolbox.AI.Anthropic;

internal class Mapper : IMapper {
    IChatRequest IMapper.CreateRequest(IAgent agent, IChat chat) => CreateRequest((IAgent<AgentOptions>)agent, chat);
    public static ChatRequest CreateRequest(IAgent<AgentOptions> agent, IChat chat)
        => new() {
                     Model = agent.Options.Model,
                     Temperature = agent.Options.Temperature,
                     MaximumOutputTokens = agent.Options.MaximumOutputTokens,
                     StopSequences = agent.Options.StopSequences.Count == 0 ? null : [.. agent.Options.StopSequences],
                     MinimumTokenProbability = agent.Options.TokenProbabilityCutOff,
                     UseStreaming = agent.Options.UseStreaming,

                     Messages = chat.Messages.ToArray(o => new RequestMessage(o)),

                     MaximumTokenSamples = agent.Options.MaximumTokensToSample,
                     System = CreateSystemMessage(agent, chat),
                 };

    private static string CreateSystemMessage(IAgent<AgentOptions> agent, IChat chat) {
        var builder = new StringBuilder();
        builder.AppendLine(agent.World.ToString());
        builder.AppendLine(agent.Persona.ToString());
        builder.AppendLine(chat.Instructions.ToString());
        return builder.ToString();
    }

    Message IMapper.CreateResponseMessage(IChat chat, IChatResponse response)
        => CreateResponseMessage(chat, (ChatResponse)response);
    public static Message CreateResponseMessage(IChat chat, ChatResponse response) {
        chat.TotalTokens += (uint)(response.Usage.InputTokens + response.Usage.OutputTokens);
        return new("assistant", response.Completion.ToArray(i => new MessagePart(i.Type, ((object?)i.Text ?? i.Image)!)));
    }
}
