namespace Sophia.Services;

public interface IPersonasService {
    Task<IReadOnlyList<PersonaData>> GetList(string? filter = null);
    Task<PersonaData?> GetById(int id);
    Task Add(PersonaData input);
    Task Update(PersonaData input);
    Task Delete(int id);
}
