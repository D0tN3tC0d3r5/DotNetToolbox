namespace Sophia.WebApp.Data.World;

[Owned]
[EntityTypeConfiguration(typeof(ArgumentEntity))]
public class ArgumentEntity
    : IEntityTypeConfiguration<ArgumentEntity> {
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(20)]
    public string Type { get; set; } = string.Empty;
    [MaxLength(2000)]
    public string? Description { get; set; }
    [MaxLength(1000)]
    public string[] Options { get; set; } = [];
    public bool IsRequired { get; set; }
    public void Configure(EntityTypeBuilder<ArgumentEntity> builder)
        => builder.Property(p => p.Options)
                  .HasConversion(o => string.Join('|', o), s => s.Split('|', StringSplitOptions.None));
}
