using Sophia.Models.Skills;

namespace Sophia.Services;

public interface ISkillsService {
    Task<IReadOnlyList<SkillData>> GetList(string? filter = null);
    Task<SkillData?> GetById(int skillId);
    Task Add(SkillData selectedSkill);
    Task Update(SkillData selectedSkill);
    Task Delete(int skillId);
}
