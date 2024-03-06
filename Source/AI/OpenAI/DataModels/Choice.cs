namespace DotNetToolbox.AI.OpenAI.DataModels;

internal record Choice {
    public int Index { get; init; }
    public string? FinishReason { get; init; }
}
