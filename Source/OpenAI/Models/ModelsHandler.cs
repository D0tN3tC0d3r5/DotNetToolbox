namespace DotNetToolbox.OpenAI.Models;

internal class ModelsHandler(IConfiguration configuration, IHttpClientProvider httpClientProvider, ILogger<ModelsHandler>? logger = null)
        : IModelsHandler {
    private readonly ILogger<ModelsHandler> _logger = logger ?? NullLogger<ModelsHandler>.Instance;
    private readonly HttpClient _httpClient = httpClientProvider.GetHttpClient(opt => SetOptions(opt, configuration));

    internal static void SetOptions(HttpClientOptionsBuilder opt, IConfiguration configuration)
    {
        opt.SetBaseAddress(new("https://api.openai.com/v1/"));
        opt.UseSimpleTokenAuthentication(auth => SetAuthentication(auth: auth, configuration: configuration));
    }

    internal static void SetAuthentication(StaticTokenAuthenticationOptions auth, IConfiguration configuration)
    {
        auth.Scheme = AuthenticationScheme.Bearer;
        auth.Token = IsNotNullOrWhiteSpace(configuration.GetValue<string>("OpenAI:ApiKey"));
    }

    public async Task<Model[]> Get(ModelType type = ModelType.Chat) {
        try {
            _logger.LogDebug("Getting list of models...");
            var models = await GetModelsAsync().ConfigureAwait(false);
            var result = models
                .Where(m => m.Type == type)
                .Select(ToModel).OfType<Model>().ToArray();
            _logger.LogDebug("A list of {numberOfModels} models was found.", result.Length);
            return result;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Failed to get list of models.");
            throw;
        }
    }

    public async Task<Model?> GetById(string id) {
        try {
            _logger.LogDebug("Getting model '{id}' details...", id);
            var model = await GetModelByIdAsync(id).ConfigureAwait(false);
            var result = ToModel(model);
            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
            if (result is null) _logger.LogDebug("The model '{id}' was not found.", id);
            else _logger.LogDebug("The model '{id}' was found.", id);
            return result;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Failed to get the model '{id}' details.", id);
            throw;
        }
    }

    public async Task<bool> Delete(string id) {
        try {
            _logger.LogDebug("Deleting the model '{id}'...", id);
            var result = await DeleteModelAsync(id).ConfigureAwait(false);
            _logger.LogDebug("The model '{id}' was deleted.", id);
            return result;
        }
        catch (Exception ex) {
            _logger.LogError(ex, "Failed to delete the model '{id}'.", id);
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
                   Name = input.Id,
                   IsFineTuned = input.IsFineTuned,
                   Type = input.Type,
                   CreatedOn = DateTimeOffset.FromUnixTimeSeconds(input.Created),
                   OwnedBy = input.OwnedBy,
               };
}
