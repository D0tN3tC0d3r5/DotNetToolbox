namespace Sophia.WebClient.Services;

public class AgentRemoteService(HttpClient httpClient)
    : IAgentRemoteService {
    public async Task<string> GetResponse(string userMessage) {
        var response = await httpClient.PostAsJsonAsync("api/agent", userMessage);
        response.EnsureSuccessStatusCode();
        var agentResponse = await response.Content.ReadFromJsonAsync<string>();
        return agentResponse!;
    }
}
