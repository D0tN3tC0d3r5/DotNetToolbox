namespace DotNetToolbox.OpenAI.DataModels;

internal record Model {
    public required string Id { get; init; }
    public long Created { get; init; }
    public string? OwnedBy { get; init; }
    public bool IsFineTuned => Id.StartsWith("ft:");
    public ModelType Type {
        get {
            var name = IsFineTuned ? Id[3..] : Id;
            return name switch {
                _ when name.StartsWith("dall-e") => ModelType.DallE,
                _ when name.StartsWith("whisper") => ModelType.Whisper,
                _ when name.StartsWith("tts") => ModelType.TextToSpeech,
                _ when name.StartsWith("text-embedding") => ModelType.Embedding,
                _ when name.StartsWith("text-moderation") => ModelType.Moderation,
                _ => ModelType.Chat,
            };
        }
    }
}
