//namespace Sophia.WebApp.Data.Providers;

//[Table("Models")]
//[EntityTypeConfiguration(typeof(ModelEntity))]
//public class ModelEntity
//    : IEntityTypeConfiguration<ModelEntity> {
//    public int ProviderId { get; set; }
//    public ProviderEntity Provider { get; set; } = default!;

//    [Required]
//    [MaxLength(50)]
//    public string Key { get; set; } = default!;

//    [Required]
//    [MaxLength(50)]
//    public string Name { get; set; } = default!;

//    public void Configure(EntityTypeBuilder<ModelEntity> builder)
//        => builder.HasKey(m => new { m.ProviderId, m.Key });

//    public ModelData ToDto()
//        => new() {
//            Key = Key,
//            Name = Name,
//        };
//}
