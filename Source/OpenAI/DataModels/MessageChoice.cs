namespace DotNetToolbox.OpenAI.DataModels;

internal record MessageChoice : Choice {
    public required Completion Message { get; init; }
}
