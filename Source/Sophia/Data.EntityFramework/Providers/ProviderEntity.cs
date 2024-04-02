namespace Sophia.Data.Providers;

[Table("Providers")]
[EntityTypeConfiguration(typeof(ProviderEntity))]
public class ProviderEntity
    : ISimpleKeyEntity<ProviderEntity, int>,
      IEntityTypeConfiguration<ProviderEntity>,
      IHasModels {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    public List<ModelEntity> Models { get; set; } = [];

    public void Configure(EntityTypeBuilder<ProviderEntity> builder) {
        builder.HasKey(p => p.Id);
        builder.HasMany(p => p.Models)
               .WithOne(m => m.Provider)
               .HasForeignKey(m => m.ProviderId)
               .OnDelete(DeleteBehavior.ClientCascade);
    }
}
