namespace DotNetToolbox.AI.OpenAI.Chats;

[method: SetsRequiredMembers]
public class ChatRequest(IChat chat, World world, UserProfile userProfile, IAgent agent)
    : IChatRequest {
    string IChatRequest.Context => (string?)Messages[0].Content ?? string.Empty;
    IEnumerable<IChatRequestMessage> IChatRequest.Messages => Messages.Skip(1).ToArray();

    [JsonPropertyName("model")]
    public required string Model { get; init; } = agent.AgentModel.Model.Id;
    [JsonPropertyName("messages")]
    public ChatRequestMessage[] Messages { get; } = SetMessages(chat, world, userProfile, agent);

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

    private static ChatRequestMessage[] SetMessages(IChat chat, World world, UserProfile userProfile, IAgent agent) {
        var systemMessage = SetSystemMessage(chat, world, userProfile, agent);
        return [new(systemMessage), .. chat.Messages.Select(m => new ChatRequestMessage(m))];
    }

    private static string SetSystemMessage(IChat chat, World world, UserProfile userProfile, IAgent agent) {
        var builder = new StringBuilder();
        builder.AppendLine(world.ToString());
        builder.AppendLine(userProfile.ToString());
        builder.AppendLine(agent.Persona.ToString());
        builder.AppendLine(chat.Instructions.ToString());
        return builder.ToString();
    }

    private static uint SetMaximumOutputTokens(IAgent agent)
        => agent.AgentModel.MaximumOutputTokens > AgentModel.MinimumOutputTokens
        && agent.AgentModel.MaximumOutputTokens < agent.AgentModel.Model.MaximumOutputTokens
            ? agent.AgentModel.MaximumOutputTokens
            : agent.AgentModel.Model.MaximumOutputTokens;
}
