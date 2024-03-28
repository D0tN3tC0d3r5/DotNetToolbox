namespace Sophia.WebApp.Data.Providers;

[Table("Models")]
[EntityTypeConfiguration(typeof(ModelEntity))]
public class ModelEntity
    : IEntityTypeConfiguration<ModelEntity> {
    public int ProviderId { get; set; }
    public ProviderEntity Provider { get; set; } = default!;

    [Required]
    [MaxLength(50)]
    public string ModelId { get; set; } = default!;

    [MaxLength(50)]
    public string? Name { get; set; }

    public void Configure(EntityTypeBuilder<ModelEntity> builder)
        => builder.HasKey(m => new { m.ProviderId, m.ModelId });

    public ModelData ToDto()
        => new() {
            Id = ModelId,
            Name = Name,
        };
}
