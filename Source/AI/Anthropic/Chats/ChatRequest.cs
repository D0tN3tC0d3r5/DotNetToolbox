namespace DotNetToolbox.AI.Anthropic.Chats;

[method: SetsRequiredMembers]
public class ChatRequest(IAgent connection, IModel model, IMessages chat)
    : IChatRequest {
    string IChatRequest.Context => System;
    IEnumerable<IChatRequestMessage> IChatRequest.Messages => [.. Messages];

    [JsonPropertyName("model")]
    public string Model { get; } = model.Id;

    [JsonPropertyName("system")]
    public string System { get; } = SetContext(chat);

    [JsonPropertyName("messages")]
    public ChatRequestMessage[] Messages { get; } = SetMessages(chat);

    [JsonPropertyName("max_tokens")]
    public uint MaximumOutputTokens { get; } = SetMaximumOutputTokens(connection, model);

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

    private static string SetContext(IMessages chat)
        => chat.Messages.First(m => m.Role == MessageRole.System).Text;

    private static ChatRequestMessage[] SetMessages(IMessages chat)
        => chat.Messages.Where(m => m.Role != MessageRole.System).ToArray(m => new ChatRequestMessage(m));

    private static uint SetMaximumOutputTokens(IAgent agent, IModel model)
        => agent.Settings.MaximumOutputTokens > AgentSettings.MinimumOutputTokens
        && agent.Settings.MaximumOutputTokens < model.MaximumOutputTokens
               ? agent.Settings.MaximumOutputTokens
               : model.MaximumOutputTokens;
}
