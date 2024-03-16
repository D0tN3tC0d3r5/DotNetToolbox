using Sophia.Models.Skills;

namespace Sophia.WebApp.Data.World;

[EntityTypeConfiguration(typeof(SkillEntity))]
public class SkillEntity
    : IEntityTypeConfiguration<SkillEntity> {
    [Key]
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(1000)]
    public string? Description { get; set; }
    public List<ArgumentEntity> Arguments { get; set; } = [];
    public void Configure(EntityTypeBuilder<SkillEntity> builder) {
        builder.OwnsMany(s => s.Arguments);
    }

    public SkillData ToDto()
        => new() {
                     Id = Id,
                     Name = Name,
                     Description = Description,
                     Arguments = Arguments.ToList(a => a.ToDto()),
                 };
}
