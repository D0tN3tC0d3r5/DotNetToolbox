namespace Sophia.Services;

public interface IAgentService {
    Task<string> GetResponse(GetResponseRequest request);
}
