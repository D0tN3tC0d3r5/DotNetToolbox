namespace DotNetToolbox.AI.OpenAI.Model;

public record ModelsResponse {
    public required Model[] Data { get; init; }
}
