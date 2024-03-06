namespace DotNetToolbox.AI.OpenAI.DataModels;

internal record MessageChoice : Choice {
    public required Completion Message { get; init; }
}
