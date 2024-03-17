using Sophia.Models.Skills;

namespace Sophia.Services;

public interface ISkillsService {
    Task<IReadOnlyList<SkillData>> GetList(string? filter = null);
    Task<SkillData?> GetById(int id);
    Task Add(SkillData selectedSkill);
    Task Update(SkillData input);
    Task Delete(int id);
}
