namespace DotNetToolbox.AI.Anthropic;

public class Mapper()
    : IMapper {
    IChatRequest IMapper.CreateRequest(IStandardAgent agent, IChat chat) => CreateRequest(agent, chat);
    public static ChatRequest CreateRequest(IStandardAgent agent, IChat chat)
        => new() {
                     Model = agent.Options.Model,
                     Temperature = agent.Options.Temperature,
                     MaximumOutputTokens = agent.Options.MaximumOutputTokens,
                     StopSequences = agent.Options.StopSequences.Count == 0 ? null : [.. agent.Options.StopSequences],
                     MinimumTokenProbability = agent.Options.TokenProbabilityCutOff,
                     UseStreaming = agent.Options.UseStreaming,
                     Messages = chat.Messages.ToArray(o => new RequestMessage(o)),
                     System = CreateSystemMessage(agent, chat),
                 };

    private static string CreateSystemMessage(IStandardAgent agent, IChat chat) {
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
