namespace DotNetToolbox.AI.OpenAI.DataModels;

internal record DeltaChoice : Choice {
    public required Message Delta { get; init; }
}
