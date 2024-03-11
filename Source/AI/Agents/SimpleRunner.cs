namespace DotNetToolbox.AI.Agents;

public abstract class SimpleRunner<TRunner, TOptions, TApiRequest, TApiResponse>(
    IAgent agent,
    World world,
    IHttpClientProvider httpClientProvider,
    ILogger<TRunner> logger)
    : BaseRunner<TRunner, TOptions, TApiRequest, TApiResponse>(agent, world, httpClientProvider, logger),
      IAgentRunner
    where TRunner : SimpleRunner<TRunner, TOptions, TApiRequest, TApiResponse>
    where TOptions : class, IAgentOptions, new()
    where TApiRequest : class
    where TApiResponse : class {

    public override Task ReceiveRequest(IRequestSource source, IChat chat, CancellationToken ct)
        => ProcessRequest(source, chat, ct);

    public override Task ReceiveResponse(string chatId, Message response, CancellationToken ct)
        => ProcessResponse(chatId, response, ct);
}
