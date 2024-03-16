
namespace Sophia.WebApp.Services;

public class SkillsService(ApplicationDbContext dbContext)
    : ISkillsService {
    public async Task<IReadOnlyList<SkillData>> GetList(string? filter = null)
        => await dbContext.Skills
                          .AsNoTracking()
                          .Select(s => s.ToDto()).ToArrayAsync();

    public async Task<SkillData?> GetById(int skillId) {
        var entity = await dbContext.Skills.AsNoTracking()
                              .FirstOrDefaultAsync(s => s.Id == skillId);
        return entity?.ToDto();
    }

    public async Task Add(SkillData selectedSkill) {
        var entity = selectedSkill.ToEntity();
        dbContext.Skills.Add(entity);
        await dbContext.SaveChangesAsync();
        selectedSkill.Id = entity.Id;
    }

    public async Task Update(SkillData selectedSkill) {
        var entity = await dbContext.Skills
                                    .FirstOrDefaultAsync(s => s.Id == selectedSkill.Id);
        entity?.UpdateFrom(selectedSkill);
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(int skillId) {
        var entity = await dbContext.Skills
                                    .FirstOrDefaultAsync(s => s.Id == skillId);
        if (entity is null) return;
        dbContext.Skills.Remove(entity);
        await dbContext.SaveChangesAsync();
    }
}
