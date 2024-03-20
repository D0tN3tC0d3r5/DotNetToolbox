namespace Sophia.Services;

public interface IPersonasRemoteService : IPersonasService;
public interface IPersonasService {
    Task<IReadOnlyList<PersonaData>> GetList(string? filter = null);
    Task<PersonaData?> GetById(int id);
    Task Add(PersonaData persona);
    Task Update(PersonaData persona);
    Task Delete(int id);
}
