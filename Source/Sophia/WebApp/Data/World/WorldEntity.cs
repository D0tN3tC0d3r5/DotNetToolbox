namespace Sophia.WebApp.Data.World;

[Table("WorldFacts")]
public class WorldFactsEntity {
    public Guid WorldId { get; set; }
    public int FactId { get; set; }
}

[Table("WorldTools")]
public class WorldToolsEntity {
    public Guid WorldId { get; set; }
    public int ToolId { get; set; }
}

[Table("Worlds")]
[EntityTypeConfiguration(typeof(WorldEntity))]
public class WorldEntity
    : IEntityTypeConfiguration<WorldEntity> {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.Empty;
    [MaxLength(1000)]
    public string? Location { get; set; }
    [Required]
    public UserProfileEntity UserProfile { get; set; } = new();
    public List<FactEntity> Facts { get; set; } = [];
    public List<ToolEntity> Tools { get; set; } = [];

    public void Configure(EntityTypeBuilder<WorldEntity> builder) {
        builder.HasMany(w => w.Facts)
               .WithMany()
               .UsingEntity<WorldFactsEntity>(l => l.HasOne<FactEntity>()
                                                    .WithMany()
                                                    .HasForeignKey(e => e.FactId),
                                              r => r.HasOne<WorldEntity>()
                                                    .WithMany()
                                                    .HasForeignKey(e => e.WorldId));
        builder.ComplexProperty(w => w.UserProfile);
        builder.HasMany(p => p.Tools)
               .WithMany()
               .UsingEntity<WorldToolsEntity>(l => l.HasOne<ToolEntity>()
                                                    .WithMany()
                                                    .HasForeignKey(e => e.ToolId),
                                              r => r.HasOne<WorldEntity>()
                                                    .WithMany()
                                                    .HasForeignKey(e => e.WorldId));
    }

    public static async Task Seed(ApplicationDbContext dbContext) {
        if (await dbContext.Worlds.AnyAsync()) return;
        var world = new WorldEntity();
        dbContext.Worlds.Add(world);
    }

    public WorldData ToDto()
        => new() {
            Location = Location,
            UserProfile = UserProfile.ToDto(),
            Facts = Facts.ToList(a => a.ToDto()),
            Tools = Tools.ToList(s => s.ToDto()),
        };
}
