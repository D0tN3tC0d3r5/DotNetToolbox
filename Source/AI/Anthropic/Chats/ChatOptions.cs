namespace DotNetToolbox.AI.Anthropic.Chats;

public record ChatOptions() : IChatOptions {
    public const string DefaultSystemMessage = "You are a helpful agent.";
    public const string DefaultChatModel = "claude-v1";

    public string Model { get; init; } = DefaultChatModel;
    public string SystemMessage { get; set; } = DefaultSystemMessage;
    public uint MaximumTokensPerMessage { get; set; }
    public decimal? Temperature { get; set; }
    public int? MaximumTokensToSample { get; set; }
    public decimal? MinimumTokenProbability { get; set; }
    public bool UseStreaming { get; set; }
    public List<string> StopSequences { get; set; } = [];

    public Result Validate(IDictionary<string, object?>? context = null) => throw new NotImplementedException();
}
