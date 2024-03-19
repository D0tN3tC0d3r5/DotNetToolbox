namespace Sophia.WebApp.Data.World;

[EntityTypeConfiguration(typeof(WorldEntity))]
public class WorldEntity
    : IEntityTypeConfiguration<WorldEntity> {
    public Guid Id { get; set; }
    public DateTimeOffset DateTime { get; set; }
    [MaxLength(1000)]
    public string? Location { get; set; }

    [MaxLength(100)]
    public UserProfileEntity UserProfile { get; set; } = new();
    public List<FactEntity> Facts { get; set; } = [];
    public List<ToolEntity> Tools { get; set; } = [];

    public void Configure(EntityTypeBuilder<WorldEntity> builder) {
        builder.HasKey(w => w.Id);
        builder.OwnsMany(w => w.Facts);
        builder.HasMany(w => w.Tools)
               .WithMany()
               .UsingEntity("AvailableTools",
               l => l.HasOne(typeof(ToolEntity)).WithMany().HasForeignKey("ToolId").HasPrincipalKey(nameof(ToolEntity.Id)),
               r => r.HasOne(typeof(WorldEntity)).WithMany().HasForeignKey("WorldId").HasPrincipalKey(nameof(Id)),
               j => j.HasKey("ToolId", "WorldId"));
    }

    public WorldData ToDto()
        => new() {
            DateTime = DateTime,
            Location = Location,
            UserProfile = UserProfile.ToDto(),
            Facts = Facts.ToList(a => a.ToDto()),
            Tools = Tools.ToList(s => s.ToDto()),
        };
}
