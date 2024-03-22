namespace Sophia.WebApp.Services;

public class AgentService : IAgentService {
    public Task<string> GetResponse(GetResponseRequest request)
        => Task.FromResult("Hello.");
}
