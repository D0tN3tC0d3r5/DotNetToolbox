namespace Sophia.WebApp.Data.Tools;

[EntityTypeConfiguration(typeof(ToolEntity))]
public class ToolEntity
    : IEntityTypeConfiguration<ToolEntity> {
    [Key]
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(1000)]
    public string? Description { get; set; }
    public List<ArgumentEntity> Arguments { get; set; } = [];

    public void Configure(EntityTypeBuilder<ToolEntity> builder)
        => builder.OwnsMany(s => s.Arguments);

    public ToolData ToDto()
        => new() {
            Id = Id,
            Name = Name,
            Description = Description,
            Arguments = Arguments.ToList(a => a.ToDto()),
        };
}
