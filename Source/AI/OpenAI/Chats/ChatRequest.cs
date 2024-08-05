namespace DotNetToolbox.AI.OpenAI.Chats;

[method: SetsRequiredMembers]
public class ChatRequest(IAgent agent, IChat chat)
    : IChatRequest {
    string IChatRequest.Context => (string?)Messages[0].Content ?? string.Empty;
    IEnumerable<IChatRequestMessage> IChatRequest.Messages => Messages.Skip(1).ToArray();

    [JsonPropertyName("model")]
    public required string Model { get; init; } = agent.Model.Model.Id;
    [JsonPropertyName("messages")]
    public ChatRequestMessage[] Messages { get; } = SetMessages(chat);

    [JsonPropertyName("max_tokens")]
    public uint MaximumOutputTokens { get; set; } = SetMaximumOutputTokens(agent);

    [JsonPropertyName("frequency_penalty")]
    public decimal? FrequencyPenalty { get; set; }
    [JsonPropertyName("presence_penalty")]
    public decimal? PresencePenalty { get; set; }
    [JsonPropertyName("temperature")]
    public decimal? Temperature { get; set; }

    [JsonPropertyName("n")]
    public int? NumberOfChoices { get; set; }
    [JsonPropertyName("stop")]
    public string[]? StopSequences { get; set; }
    [JsonPropertyName("top_p")]
    public decimal? MinimumTokenProbability { get; set; }
    [JsonPropertyName("stream")]
    public bool? ResponseIsStream { get; set; }
    [JsonPropertyName("tools")]
    public ChatRequestTool[]? Tools { get; set; }
    [JsonPropertyName("tool_choice")]
    public ChatRequestForceToolCall? ForceToolCall { get; set; }
    [JsonPropertyName("response_format")]
    public ChatRequestResponseFormat? ResponseFormat { get; set; }

    private static ChatRequestMessage[] SetMessages(IChat chat)
        => chat.Messages.ToArray(m => new ChatRequestMessage(m));

    private static uint SetMaximumOutputTokens(IAgent agent)
        => agent.Model.MaximumOutputTokens > AgentModel.MinimumOutputTokens
        && agent.Model.MaximumOutputTokens < agent.Model.Model.MaximumOutputTokens
            ? agent.Model.MaximumOutputTokens
            : agent.Model.Model.MaximumOutputTokens;
}
