namespace DotNetToolbox.AI.OpenAI.Models;

public class ModelsHandler(IHttpClientProvider httpClientProvider, ILogger<ModelsHandler> logger)
        : IModelsHandler {
    private readonly HttpClient _httpClient = httpClientProvider.GetHttpClient();

    public async Task<string[]> GetIds(string? type = null) {
        try {
            logger.LogDebug("Getting list of models...");
            type ??= "chat";
            var models = await GetModelsAsync().ConfigureAwait(false);
            var result = models
                        .Where(m => GetModelType(m.Id) == type)
                        .Select(m => m.Id)
                        .ToArray();
            logger.LogDebug("A list of {NumberOfModels} models of type {Type} was found.", result.Length, type);
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

    public static string GetModelType(string id) {
        var name = id.StartsWith("ft:") ? id[3..] : id;
        return name switch {
            _ when name.StartsWith("dall-e") => "image",
            _ when name.StartsWith("whisper") => "stt",
            _ when name.StartsWith("tts") => "tts",
            _ when name.StartsWith("text-embedding") => "embedding",
            _ when name.StartsWith("text-moderation") => "moderation",
            _ => "chat",
        };
    }
}
