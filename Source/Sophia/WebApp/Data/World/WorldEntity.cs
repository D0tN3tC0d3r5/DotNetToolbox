namespace Sophia.WebApp.Data.World;

[Table("Worlds")]
[EntityTypeConfiguration(typeof(WorldEntity))]
public class WorldEntity
    : IEntityTypeConfiguration<WorldEntity> {
    public Guid Id { get; set; }
    [MaxLength(1000)]
    public string? Location { get; set; }
    public UserProfileEntity? UserProfile { get; set; }
    public List<FactEntity> Facts { get; set; } = [];
    public List<ToolEntity> Tools { get; set; } = [];

    public void Configure(EntityTypeBuilder<WorldEntity> builder) {
        builder.HasKey(w => w.Id);
        builder.OwnsMany(w => w.Facts)
               .ToTable("World_Facts");
        builder.OwnsOne(w => w.UserProfile)
               .ToTable("World_User");
        builder.HasMany(w => w.Tools)
               .WithMany()
               .UsingEntity("World_Tools",
               l => l.HasOne(typeof(ToolEntity)).WithMany().HasForeignKey("ToolId").HasPrincipalKey(nameof(ToolEntity.Id)),
               r => r.HasOne(typeof(WorldEntity)).WithMany().HasForeignKey("WorldId").HasPrincipalKey(nameof(Id)),
               j => j.HasKey("ToolId", "WorldId"));
        builder.HasData(new WorldEntity {
            Id = Guid.Parse("49381b5a-a76b-486f-ac5c-b2807cff9675"),
        });
    }

    public WorldData ToDto()
        => new() {
            Location = Location,
            UserProfile = (UserProfile ?? new()).ToDto(),
            Facts = Facts.ToList(a => a.ToDto()),
            Tools = Tools.ToList(s => s.ToDto()),
        };
}
