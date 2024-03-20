namespace Sophia.WebApp.Data.Common;

[Table("Facts")]
[EntityTypeConfiguration(typeof(FactEntity))]
public class FactEntity
    : IEntityTypeConfiguration<FactEntity> {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [MaxLength(1000)]
    public string? DefaultText { get; set; }

    [MaxLength(100)]
    public string? Value { get; set; }

    [MaxLength(1000)]
    [Required(AllowEmptyStrings = false)]
    public string ValueTemplate { get; set; } = "{0}";

    public void Configure(EntityTypeBuilder<FactEntity> builder)
        => builder.Property(f => f.ValueTemplate)
                  .HasDefaultValue("{0}");

    public FactData ToDto()
        => new() {
            DefaultText = DefaultText,
            Value = Value,
            ValueTemplate = ValueTemplate,
        };
}
