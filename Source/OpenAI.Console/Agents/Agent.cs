namespace DotNetToolbox.Sophia.Agents;
internal record Agent {
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required string Profile { get; init; }
    public required string[] Skills { get; init; }
    public decimal? FrequencyPenalty { get; init; }
    public decimal? PresencePenalty { get; init; }
    public string[]? StopSignals { get; init; }
    public decimal? Temperature { get; init; }
    public uint? MaximumTokensPerMessage { get; init; }
}
