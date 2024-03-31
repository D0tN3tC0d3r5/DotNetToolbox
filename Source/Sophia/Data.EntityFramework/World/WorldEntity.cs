namespace Sophia.Data.World;

[Table("Worlds")]
[EntityTypeConfiguration(typeof(WorldEntity))]
public class WorldEntity
    : IEntity<Guid>,
      IEntityTypeConfiguration<WorldEntity>,
      IHasFacts<Guid> {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public List<string> Facts { get; set; } = [];

    public void Configure(EntityTypeBuilder<WorldEntity> builder)
        => builder.PrimitiveCollection(w => w.Facts);
}
