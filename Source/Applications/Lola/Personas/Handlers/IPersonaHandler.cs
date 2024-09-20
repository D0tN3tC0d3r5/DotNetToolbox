using Task = System.Threading.Tasks.Task;

namespace Lola.Personas.Handlers;

public interface IPersonaHandler {
    PersonaEntity[] List();
    PersonaEntity? GetById(uint id);
    PersonaEntity? Find(Expression<Func<PersonaEntity, bool>> predicate);
    void Add(PersonaEntity persona);
    void Update(PersonaEntity persona);
    void Remove(uint id);

    Task<Query[]> GenerateQuestion(PersonaEntity persona);
    Task UpdateCreatedPersona(PersonaEntity persona);
}
