namespace Sophia.Services;

public interface IToolsRemoteService : IToolsService;
public interface IToolsService {
    Task<IReadOnlyList<ToolData>> GetList(string? filter = null);
    Task<ToolData?> GetById(int id);
    Task Add(ToolData selectedTool);
    Task Update(ToolData input);
    Task Delete(int id);
}
