namespace Sophia.WebClient.Services;

public class AgentRemoteService(HttpClient httpClient)
    : IAgentRemoteService {
    public async Task<string> GetResponse(GetResponseRequest request) {
        var response = await httpClient.PostAsJsonAsync("api/agent", request);
        response.EnsureSuccessStatusCode();
        var agentResponse = await response.Content.ReadAsStringAsync();
        return agentResponse!;
    }
}
