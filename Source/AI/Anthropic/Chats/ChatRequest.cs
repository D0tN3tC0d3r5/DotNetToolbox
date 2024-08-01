namespace DotNetToolbox.AI.Anthropic.Chats;
public class ChatRequest : IChatRequest {
    [SetsRequiredMembers]
    public ChatRequest(IChat chat, World world, UserProfile userProfile, IAgent agent) {
        Model = agent.AgentModel.Model.Id;
        Context = SetSystemMessage(chat, world, userProfile, agent);
        Messages = SetMessageHistory(chat);
        MaximumOutputTokens = agent.AgentModel.MaximumOutputTokens > AgentModel.MinimumOutputTokens
                           && agent.AgentModel.MaximumOutputTokens < agent.AgentModel.Model.MaximumOutputTokens
                                  ? agent.AgentModel.MaximumOutputTokens
                                  : agent.AgentModel.Model.MaximumOutputTokens;
    }

    [JsonPropertyName("model")]
    public required string Model { get; init; }

    [JsonPropertyName("system")]
    public required string Context { get; init; }
    IEnumerable<IChatRequestMessage> IChatRequest.Messages => Messages.ToArray<IChatRequestMessage>();
    [JsonPropertyName("messages")]
    public required ChatRequestMessage[] Messages { get; init; }
    [JsonPropertyName("max_tokens")]
    public uint MaximumOutputTokens { get; init; }

    [JsonPropertyName("metadata")]
    public ChatMetadata? Metadata { get; set; }
    [JsonPropertyName("stop_sequences")]
    public string[]? StopSequences { get; set; }
    [JsonPropertyName("stream")]
    public bool? ResponseIsStream { get; set; }
    [JsonPropertyName("temperature")]
    public decimal? Temperature { get; set; }
    [JsonPropertyName("top_p")]
    public decimal? MinimumTokenProbability { get; set; }

    [JsonPropertyName("top_k")]
    public decimal? MaximumTokenSamples { get; set; }

    private string SetSystemMessage(IChat chat, World world, UserProfile userProfile, IAgent agent) {
        var builder = new StringBuilder();
        builder.AppendLine(world.ToString());
        builder.AppendLine(userProfile.ToString());
        builder.AppendLine(agent.Persona.ToString());
        builder.AppendLine(chat.Instructions.ToString());
        return builder.ToString();
    }

    private ChatRequestMessage[] SetMessageHistory(IChat chat)
        => chat.Messages.Where(m => m.Role is Roles.User or Roles.Assistant).ToArray(m => new ChatRequestMessage(m));
}
