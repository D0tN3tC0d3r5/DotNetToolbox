namespace DotNetToolbox.AI.Agents;

public abstract class BackgroundAgent<TAgent, TMapper, TRequest, TResponse>(string provider,
                                                                            IHttpClientProviderFactory factory,
                                                                            ILogger<TAgent> logger)
    : Agent<TAgent, TMapper, TRequest, TResponse>(provider, factory, logger),
      IBackgroundAgent
    where TAgent : BackgroundAgent<TAgent, TMapper, TRequest, TResponse>
    where TMapper : class, IMapper, new()
    where TRequest : class, IChatRequest
    where TResponse : class, IChatResponse {

    // this should be a fire and forget method.
    // Use the cancellation token to stop the agent.
    public async void Run(CancellationToken ct) {
        Logger.LogInformation("Start running...");
        try {
            while (!ct.IsCancellationRequested) {
                await Execute(ct);
                await Task.Delay(100, ct);
            }
        }
        catch (OperationCanceledException ex) {
            Logger.LogWarning(ex, "Running cancellation requested!");
        }
        catch (Exception ex) {
            Logger.LogError(ex, "An error occurred while running the actor!");
            throw;
        }
        Logger.LogInformation("Running stopped.");
    }

    protected virtual Task Execute(CancellationToken token) => Task.CompletedTask;
}
