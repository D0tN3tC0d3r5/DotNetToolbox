namespace DotNetToolbox.AI.OpenAI.Models;

public record ModelsResponse {
    public required Model[] Data { get; init; }
}
