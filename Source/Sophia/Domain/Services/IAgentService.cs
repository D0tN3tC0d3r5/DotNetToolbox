namespace Sophia.Services;

public interface IAgentRemoteService : IAgentService;

public interface IAgentService {
    Task<string> GetResponse(string userMessage);
}
