namespace DotNetToolbox.OpenAI.DataModels;

internal record MessageChoice : Choice {
    public required Message Message { get; init; }
}
