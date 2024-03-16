namespace DotNetToolbox.AI.Agents;

public abstract class BackgroundAgent<TAgent, TOptions, TRequest, TResponse>(
        World world,
        TOptions options,
        Persona persona,
        IMapper mapper,
        IHttpClientProvider httpClientProvider,
        ILogger<TAgent> logger)
    : Agent<TAgent, TOptions, TRequest, TResponse>(world, options, persona, mapper, httpClientProvider, logger),
      IBackgroundAgent
    where TAgent : BackgroundAgent<TAgent, TOptions, TRequest, TResponse>
    where TOptions : class, IAgentOptions, new()
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
