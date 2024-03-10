using DotNetToolbox.AI.Agents;

namespace DotNetToolbox.AI.Chats;

public abstract class ChatHandler<TChatHandler, TOptions, TRequest, TResponse>(
    World world,
    TOptions options,
    IChat chat,
    IHttpClientProvider httpClientProvider)
    : IChatHandler
    where TChatHandler : ChatHandler<TChatHandler, TOptions, TRequest, TResponse>
    where TOptions : class, IChatOptions, new() {
    protected World World { get; } = world;
    protected TOptions Options { get; } = options;
    protected IChat Chat { get; } = chat;

    public virtual async Task<HttpResult> Submit(IAgent agent, CancellationToken ct = default) {
        var request = CreateRequest(agent);
        var content = JsonContent.Create(request, options: IChatOptions.SerializerOptions);
        var httpClient = httpClientProvider.GetHttpClient();
        var httpResult = await httpClient.PostAsync(Options.ApiEndpoint, content, ct).ConfigureAwait(false);
        try {
            httpResult.EnsureSuccessStatusCode();
            var json = await httpResult.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            var response = JsonSerializer.Deserialize<TResponse>(json, IChatOptions.SerializerOptions)!;
            Chat.Messages.Add(CreateMessage(response));
            return HttpResult.Ok();
        }
        catch (Exception ex) {
            switch (httpResult.StatusCode) {
                case HttpStatusCode.BadRequest:
                    var response = await httpResult.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
                    var errorMessage = $"""
                                        Package: {JsonSerializer.Serialize(request, IChatOptions.SerializerOptions)}
                                        ResponseContent: {response};
                                        """;
                    return HttpResult.BadRequest(errorMessage);
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                    return HttpResult.Unauthorized();
                case HttpStatusCode.NotFound:
                    return HttpResult.NotFound();
                default:
                    return HttpResult.InternalError(ex);
            }
        }
    }

    protected abstract TRequest CreateRequest(IAgent agent);
    protected abstract string CreateSystemMessage(World world, IAgent agent);
    protected abstract Message CreateMessage(TResponse response);
}
