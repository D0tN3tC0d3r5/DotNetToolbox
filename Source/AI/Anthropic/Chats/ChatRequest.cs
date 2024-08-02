namespace DotNetToolbox.AI.Anthropic.Chats;

[method: SetsRequiredMembers]
public class ChatRequest(IChat chat, World world, UserProfile userProfile, IAgent agent)
    : IChatRequest {
    string IChatRequest.Context => System;
    IEnumerable<IChatRequestMessage> IChatRequest.Messages => Messages.ToArray<IChatRequestMessage>();

    [JsonPropertyName("model")]
    public string Model { get; } = agent.AgentModel.Model.Id;

    [JsonPropertyName("system")]
    public string System { get; } = SetContext(chat, world, userProfile, agent);

    [JsonPropertyName("messages")]
    public ChatRequestMessage[] Messages { get; } = SetMessages(chat);

    [JsonPropertyName("max_tokens")]
    public uint MaximumOutputTokens { get; } = SetMaximumOutputTokens(agent);

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

    private static string SetContext(IChat chat, World world, UserProfile userProfile, IAgent agent) {
        var builder = new StringBuilder();
        builder.AppendLine(world.ToString());
        builder.AppendLine(userProfile.ToString());
        builder.AppendLine(agent.Persona.ToString());
        builder.AppendLine(chat.Instructions.ToString());
        return builder.ToString();
    }

    private static ChatRequestMessage[] SetMessages(IChat chat)
        => chat.Messages.ToArray(m => new ChatRequestMessage(m));

    private static uint SetMaximumOutputTokens(IAgent agent)
        => agent.AgentModel.MaximumOutputTokens > AgentModel.MinimumOutputTokens
        && agent.AgentModel.MaximumOutputTokens < agent.AgentModel.Model.MaximumOutputTokens
               ? agent.AgentModel.MaximumOutputTokens
               : agent.AgentModel.Model.MaximumOutputTokens;
}
