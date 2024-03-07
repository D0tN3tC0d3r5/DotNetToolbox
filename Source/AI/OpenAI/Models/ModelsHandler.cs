namespace DotNetToolbox.AI.OpenAI.Models;

internal class ModelsHandler(IHttpClientProvider httpClientProvider, ILogger<ModelsHandler> logger)
        : IModelsHandler {
    private readonly HttpClient _httpClient = httpClientProvider.GetHttpClient();

    public async Task<string[]> Get(ModelType type = ModelType.Chat) {
        try {
            logger.LogDebug("Getting list of models...");
            var models = await GetModelsAsync().ConfigureAwait(false);
            var result = models
                        .Where(m => GetModelType(m.Id) == type)
                        .Select(m => m.Id).ToArray();
            logger.LogDebug("A list of {numberOfModels} models was found.", result.Length);
            return result;
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to get list of models.");
            throw;
        }
    }

    private async Task<Model[]> GetModelsAsync() {
        var response = await _httpClient.GetAsync("models").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ModelsResponse>().ConfigureAwait(false);
        return result!.Data;
    }

    public static ModelType GetModelType(string id) {
        var name = id.StartsWith("ft:") ? id[3..] : id;
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
