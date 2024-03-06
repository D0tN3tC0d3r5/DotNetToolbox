namespace DotNetToolbox.AI.Anthropic;

public record ClaudeChatOptions : ChatOptions {
    public virtual string Model { get; set; } = "claude-v1";
    public virtual int? MaxTokensToSample { get; set; }
    public virtual List<string>? StopSequences { get; set; }
}