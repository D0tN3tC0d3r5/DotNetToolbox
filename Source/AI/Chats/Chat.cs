
using System.Net;

namespace DotNetToolbox.AI.Chats;

public abstract class Chat<TChat, TOptions, TRequest, TResponse>
    : IChat
    where TChat : Chat<TChat, TOptions, TRequest, TResponse>
    where TOptions : class, IChatOptions, new() {
    private readonly IHttpClientProvider _httpClientProvider;

    protected Chat(IHttpClientProvider httpClientProvider, TOptions? options) {
        _httpClientProvider = httpClientProvider;
        Options = options ?? Options;
    }

    public string Id { get; } = Guid.NewGuid().ToString();
    public int TotalNumberOfTokens { get; set; }

    IChatOptions IChat.Options => Options;
    public TOptions Options { get; } = new();
    public List<Message> Messages { get; } = [];

    public virtual async Task<HttpResult> Submit(CancellationToken ct = default) {
        var request = CreateRequest();
        var content = JsonContent.Create(request, options: IChatOptions.SerializerOptions);
        var httpClient = _httpClientProvider.GetHttpClient();
        var httpResult = await httpClient.PostAsync(Options.ApiEndpoint, content, ct).ConfigureAwait(false);
        try {
            httpResult.EnsureSuccessStatusCode();
            var json = await httpResult.Content.ReadAsStringAsync(ct).ConfigureAwait(false);
            var response = JsonSerializer.Deserialize<TResponse>(json, IChatOptions.SerializerOptions)!;
            Messages.Add(CreateMessage(response));
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

    protected abstract TRequest CreateRequest();
    protected abstract Message CreateMessage(TResponse response);
}
