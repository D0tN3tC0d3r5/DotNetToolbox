namespace Sophia.WebApp.Data.World;

[Table("Worlds")]
[EntityTypeConfiguration(typeof(WorldEntity))]
public class WorldEntity
    : IEntityTypeConfiguration<WorldEntity>
    , IHasFacts<Guid> {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; } = Guid.Empty;
    public HashSet<string> Facts { get; set; } = [];

    public void Configure(EntityTypeBuilder<WorldEntity> builder)
        => builder.PrimitiveCollection(w => w.Facts);

    public static async Task Seed(ApplicationDbContext dbContext) {
        if (await dbContext.Worlds.AnyAsync()) return;
        var world = new WorldEntity();
        dbContext.Worlds.Add(world);
    }

    public WorldData ToDto()
        => new() {
            Facts = Facts,
        };
}
