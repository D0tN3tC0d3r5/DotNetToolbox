namespace DotNetToolbox.OpenAI.Models;

internal class ModelsHandler(IHttpClientProvider httpClientProvider, ILogger<ModelsHandler> logger)
        : IModelsHandler {
    private readonly HttpClient _httpClient = httpClientProvider.GetHttpClient();

    public async Task<Model[]> Get(ModelType type = ModelType.Chat) {
        try {
            logger.LogDebug("Getting list of models...");
            var models = await GetModelsAsync().ConfigureAwait(false);
            var result = models
                .Where(m => m.Type == type)
                .Select(ToModel).OfType<Model>().ToArray();
            logger.LogDebug("A list of {numberOfModels} models was found.", result.Length);
            return result;
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to get list of models.");
            throw;
        }
    }

    public async Task<Model?> GetById(string id) {
        try {
            logger.LogDebug("Getting model '{id}' details...", id);
            var model = await GetModelByIdAsync(id).ConfigureAwait(false);
            var result = ToModel(model);
            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
            if (result is null) logger.LogDebug("The model '{id}' was not found.", id);
            else logger.LogDebug("The model '{id}' was found.", id);
            return result;
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to get the model '{id}' details.", id);
            throw;
        }
    }

    public async Task<bool> Delete(string id) {
        try {
            logger.LogDebug("Deleting the model '{id}'...", id);
            var result = await DeleteModelAsync(id).ConfigureAwait(false);
            logger.LogDebug("The model '{id}' was deleted.", id);
            return result;
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to delete the model '{id}'.", id);
            throw;
        }
    }

    private async Task<OpenAiModel[]> GetModelsAsync() {
        var response = await _httpClient.GetAsync("models").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ModelsResponse>().ConfigureAwait(false);
        return result!.Data;
    }

    private async Task<OpenAiModel?> GetModelByIdAsync(string id) {
        var response = await _httpClient.GetAsync($"models/{id}").ConfigureAwait(false);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<OpenAiModel>().ConfigureAwait(false);
        return result;
    }

    private async Task<bool> DeleteModelAsync(string id) {
        var response = await _httpClient.DeleteAsync($"models/{id}").ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<DeleteResponse>().ConfigureAwait(false);
        return result!.Deleted;
    }

    [return: NotNullIfNotNull(nameof(input))]
    private static Model? ToModel(OpenAiModel? input)
        => input is null
               ? null
               : new() {
                   Id = input.Id,
                   Name = input.IsFineTuned ? input.Id[3..] : input.Id,
                   IsFineTuned = input.IsFineTuned,
                   Type = input.Type,
                   CreatedOn = DateTimeOffset.FromUnixTimeSeconds(input.Created),
                   OwnedBy = input.OwnedBy,
               };
}
