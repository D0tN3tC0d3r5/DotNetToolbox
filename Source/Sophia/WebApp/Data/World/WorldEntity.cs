using Sophia.Models.Worlds;

namespace Sophia.WebApp.Data.World;

[EntityTypeConfiguration(typeof(WorldEntity))]
public class WorldEntity
    : IEntityTypeConfiguration<WorldEntity> {
    public Guid Id { get; set; }
    public DateTimeOffset DateTime { get; set; }
    [MaxLength(1000)]
    public string? Location { get; set; }
    [MaxLength(100)]
    public string? UserProfile { get; set; }
    public List<InformationEntity> CustomValues { get; set; } = [];
    public List<SkillEntity> Skills { get; set; } = [];

    public void Configure(EntityTypeBuilder<WorldEntity> builder) {
        builder.HasKey(w => w.Id);
        builder.OwnsMany(w => w.CustomValues);
        builder.HasMany(w => w.Skills)
               .WithMany()
               .UsingEntity("AvailableSkills",
               l => l.HasOne(typeof(SkillEntity)).WithMany().HasForeignKey("SkillId").HasPrincipalKey(nameof(SkillEntity.Id)),
               r => r.HasOne(typeof(WorldEntity)).WithMany().HasForeignKey("WorldId").HasPrincipalKey(nameof(Id)),
               j => j.HasKey("SkillId", "WorldId"));
    }

    public WorldData ToDto()
        => new() {
            DateTime = DateTime,
            Location = Location,
            UserProfile = UserProfile,
            AdditionalInformation = CustomValues.ToList(a => a.ToDto()),
            Skills = Skills.ToList(s => s.ToDto()),
        };
}
