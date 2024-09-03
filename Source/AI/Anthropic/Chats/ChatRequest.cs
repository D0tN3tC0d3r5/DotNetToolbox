namespace DotNetToolbox.AI.Anthropic.Chats;

[method: SetsRequiredMembers]
public class ChatRequest(IAgent agent, IChat chat)
    : IChatRequest {
    string IChatRequest.Context => System;
    IEnumerable<IChatRequestMessage> IChatRequest.Messages => [.. Messages];

    [JsonPropertyName("model")]
    public string Model { get; } = agent.Settings.Model.Id;

    [JsonPropertyName("system")]
    public string System { get; } = SetContext(chat);

    [JsonPropertyName("messages")]
    public ChatRequestMessage[] Messages { get; } = SetMessages(chat);

    [JsonPropertyName("max_tokens")]
    public uint MaximumOutputTokens { get; } = SetMaximumOutputTokens(agent);

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

    private static string SetContext(IChat chat)
        => chat.Messages.First(m => m.Role == MessageRole.System).Text;

    private static ChatRequestMessage[] SetMessages(IChat chat)
        => chat.Messages.Where(m => m.Role != MessageRole.System).ToArray(m => new ChatRequestMessage(m));

    private static uint SetMaximumOutputTokens(IAgent agent)
        => agent.Settings.MaximumOutputTokens > AgentSettings.MinimumOutputTokens
        && agent.Settings.MaximumOutputTokens < agent.Settings.Model.MaximumOutputTokens
               ? agent.Settings.MaximumOutputTokens
               : agent.Settings.Model.MaximumOutputTokens;
}
