namespace Sophia.WebApp.Services;

public class AgentService : IAgentService {
    public Task<string> GetResponse(string userMessage)
        => Task.FromResult("Hello.");
}
