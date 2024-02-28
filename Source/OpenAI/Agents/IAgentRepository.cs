namespace DotNetToolbox.OpenAI.Agents;

internal interface IAgentRepository : IDisposable {
    Task<Agent[]> Get(CancellationToken ct = default);
    Task<Agent?> GetById(string id, CancellationToken ct = default);
    Task Add(Agent chat, CancellationToken ct = default);
    Task Delete(string id, CancellationToken ct = default);
}
