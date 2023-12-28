namespace DotNetToolbox.OpenAI.Chats;

public record ChatOptions(string Model = "gpt-3.5-turbo-1106") {
    public virtual decimal? FrequencyPenalty { get; init; }
    public virtual int? MaximumNumberOfTokensPerMessage { get; init; }
    public virtual int? NumberOfChoices { get; init; }
    public virtual int? PresencePenalty { get; init; }
    public virtual string[] StopSignals { get; init; } = [];
    public virtual decimal? Temperature { get; init; }
    public virtual int? TopProbability { get; init; }
    public virtual Tool[] Tools { get; init; } = [];
    public virtual bool UseStream { get; init; }
}
