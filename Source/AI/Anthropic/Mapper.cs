using DotNetToolbox.AI.Common;

namespace DotNetToolbox.AI.Anthropic;

public class Mapper()
    : IMapper {
    IChatRequest IMapper.CreateRequest(IChat chat, World world, UserProfile user, IAgent agent) => CreateRequest(chat, world, user, agent);
    public static ChatRequest CreateRequest(IChat chat, World world, UserProfile user, IAgent agent)
        => new() {
            Model = agent.Model.Id,
            Temperature = agent.Model.Temperature,
            MaximumOutputTokens = agent.Model.MaximumOutputTokens,
            StopSequences = agent.Model.StopSequences.Count == 0 ? null : [.. agent.Model.StopSequences],
            MinimumTokenProbability = agent.Model.TokenProbabilityCutOff,
            ResponseIsStream = agent.Model.ResponseIsStream,
            Messages = chat.Messages.ToArray(o => new RequestMessage(o)),
            System = CreateSystemMessage(chat, world, user, agent),
        };

    private static string CreateSystemMessage(IChat chat, World world, UserProfile user, IAgent agent) {
        var builder = new StringBuilder();
        builder.AppendLine(world.ToString());
        builder.AppendLine(user.ToString());
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
