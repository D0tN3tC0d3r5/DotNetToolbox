namespace DotNetToolbox.OpenAI.DataModels;

internal record Usage {
    public int PromptTokens { get; init; }
    public int CompletionTokens { get; init; }
    public int TotalTokens { get; init; }
}
