namespace DotNetToolbox.AI.Anthropic;

public class Mapper()
    : IMapper {
    IChatRequest IMapper.CreateRequest(IChat chat, World world, UserProfile userProfile, IAgent agent) => CreateRequest(chat, world, userProfile, agent);
    public static ChatRequest CreateRequest(IChat chat, World world, UserProfile userProfile, IAgent agent)
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

    private static string CreateSystemMessage(IChat chat, World world, UserProfile userProfile, IAgent agent) {
        var builder = new StringBuilder();
        builder.AppendLine(world.ToString());
        builder.AppendLine(userProfile.ToString());
        builder.AppendLine(agent.Persona.ToString());
        builder.AppendLine(chat.Instructions.ToString());
        return builder.ToString();
    }

    Message IMapper.CreateResponseMessage(IChat chat, IChatResponse response)
        => CreateResponseMessage(chat, (ChatResponse)response);
    public static Message CreateResponseMessage(IChat chat, ChatResponse response) {
        chat.TotalTokens += (uint)(response.Usage.InputTokens + response.Usage.OutputTokens);
        return new("assistant", response.Content.ToArray(i => new MessagePart(i.Type, ((object?)i.Text ?? i.Image)!)));
    }
}
