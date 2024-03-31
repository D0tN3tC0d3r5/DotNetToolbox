namespace Sophia.Data.Tools;

[Owned]
[Table("Arguments")]
[EntityTypeConfiguration(typeof(ArgumentEntity))]
public class ArgumentEntity
    : IEntityTypeConfiguration<ArgumentEntity> {
    [Required]
    public int ToolId { get; set; }
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Index { get; set; }
    [Required]
    public bool IsRequired { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [MaxLength(20)]
    public ArgumentType Type { get; set; }

    [MaxLength(1024)]
    public string[] Choices { get; set; } = [];
    [MaxLength(2000)]
    public string? Description { get; set; }

    public void Configure(EntityTypeBuilder<ArgumentEntity> builder) {
        builder.HasKey(a => new { a.ToolId, a.Index });
        builder.PrimitiveCollection(p => p.Choices);
    }
}
