
namespace Sophia.WebApp.Services;

public class SkillsService(ApplicationDbContext dbContext)
    : ISkillsService {
    public async Task<IReadOnlyList<SkillData>> GetList(string? filter = null)
        => await dbContext.Skills
                          .AsNoTracking()
                          .Select(s => s.ToDto()).ToArrayAsync();

    public async Task<SkillData?> GetById(int id) {
        var entity = await dbContext.Skills.AsNoTracking()
                              .FirstOrDefaultAsync(s => s.Id == id);
        return entity?.ToDto();
    }

    public async Task Add(SkillData selectedSkill) {
        var entity = selectedSkill.ToEntity();
        dbContext.Skills.Add(entity);
        await dbContext.SaveChangesAsync();
        selectedSkill.Id = entity.Id;
    }

    public async Task Update(SkillData input) {
        var entity = await dbContext.Skills
                                    .FirstOrDefaultAsync(s => s.Id == input.Id);
        entity?.UpdateFrom(input);
        await dbContext.SaveChangesAsync();
    }

    public async Task Delete(int id) {
        var entity = await dbContext.Skills
                                    .FirstOrDefaultAsync(s => s.Id == id);
        if (entity is null) return;
        dbContext.Skills.Remove(entity);
        await dbContext.SaveChangesAsync();
    }
}
