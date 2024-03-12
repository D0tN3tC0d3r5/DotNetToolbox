namespace DotNetToolbox.AI.Agents;

public abstract class BackgroundAgent<TRunner, TOptions, TApiRequest, TApiResponse>(
        World world,
        TOptions options,
        IPersona persona,
        IHttpClientProvider httpClientProvider,
        ILogger<TRunner> logger)
    : Agent<TRunner, TOptions, TApiRequest, TApiResponse>(world, options, persona, httpClientProvider, logger),
      IBackgroundAgent
    where TRunner : BackgroundAgent<TRunner, TOptions, TApiRequest, TApiResponse>
    where TOptions : class, IAgentOptions, new()
    where TApiRequest : class
    where TApiResponse : class {

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
