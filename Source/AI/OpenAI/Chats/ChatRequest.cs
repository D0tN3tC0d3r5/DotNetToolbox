namespace DotNetToolbox.AI.OpenAI.Chats;

public class ChatRequest : IChatRequest {
    [JsonPropertyName("model")]
    public required string Model { get; init; }
    public string Context => (string?)Messages.SingleOrDefault(m => m.Role == Roles.System)?.Content ?? string.Empty;
    IEnumerable<IChatRequestMessage> IChatRequest.Messages => Messages.Where(m => m.Role != Roles.System).ToArray<IChatRequestMessage>();
    [JsonPropertyName("messages")]
    public ChatRequestMessage[] Messages { get; } = [];
    [JsonPropertyName("frequency_penalty")]
    public decimal? FrequencyPenalty { get; set; }
    [JsonPropertyName("presence_penalty")]
    public decimal? PresencePenalty { get; set; }
    [JsonPropertyName("temperature")]
    public decimal? Temperature { get; set; }
    [JsonPropertyName("max_tokens")]
    public uint MaximumOutputTokens { get; set; }
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

    private string SetSystemMessage(IChat chat, World world, UserProfile userProfile, IAgent agent) {
        var builder = new StringBuilder();
        builder.AppendLine(world.ToString());
        builder.AppendLine(userProfile.ToString());
        builder.AppendLine(agent.Persona.ToString());
        builder.AppendLine(chat.Instructions.ToString());
        return builder.ToString();
    }

    public RequestMessage[] SetMessageHistory(IChat chat)
        => chat.Messages.Where(m => m.Role is Roles.User or Roles.Assistant).ToArray(m => new RequestMessage(m));
}
