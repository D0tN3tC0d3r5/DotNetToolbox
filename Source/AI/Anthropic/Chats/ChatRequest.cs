namespace DotNetToolbox.AI.Anthropic.Chats;

[method: SetsRequiredMembers]
public class ChatRequest(World world, IJob job, IAgent agent, IChat chat)
    : IChatRequest {
    string IChatRequest.Context => System;
    IEnumerable<IChatRequestMessage> IChatRequest.Messages => Messages.ToArray<IChatRequestMessage>();

    [JsonPropertyName("model")]
    public string Model { get; } = agent.Model.Model.Id;

    [JsonPropertyName("system")]
    public string System { get; } = SetContext(world, job, agent);

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

    private static string SetContext(World world, IJob job, IAgent agent) {
        var builder = new StringBuilder();
        builder.AppendLine(world.ToText(string.Empty));
        builder.AppendLine(agent.Persona.GetIndentedText(string.Empty));
        builder.AppendLine(job.Instructions);
        return builder.ToString();
    }

    private static ChatRequestMessage[] SetMessages(IChat chat)
        => chat.Messages.ToArray(m => new ChatRequestMessage(m));

    private static uint SetMaximumOutputTokens(IAgent agent)
        => agent.Model.MaximumOutputTokens > AgentModel.MinimumOutputTokens
        && agent.Model.MaximumOutputTokens < agent.Model.Model.MaximumOutputTokens
               ? agent.Model.MaximumOutputTokens
               : agent.Model.Model.MaximumOutputTokens;
}
