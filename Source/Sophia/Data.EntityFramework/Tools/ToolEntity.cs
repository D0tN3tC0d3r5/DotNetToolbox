namespace Sophia.Data.Tools;

[Table("Tools")]
[EntityTypeConfiguration(typeof(ToolEntity))]
public class ToolEntity
    : IEntityTypeConfiguration<ToolEntity> {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(1000)]
    public string? Description { get; set; }
    public List<ArgumentEntity> Arguments { get; set; } = [];

    public void Configure(EntityTypeBuilder<ToolEntity> builder)
        => builder.HasMany(s => s.Arguments)
                  .WithOne()
                  .HasForeignKey(a => a.ToolId);
}
