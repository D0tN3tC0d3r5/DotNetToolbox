namespace Sophia.WebApp.Data.Tools;

[Owned]
[EntityTypeConfiguration(typeof(ArgumentEntity))]
public class ArgumentEntity
    : IEntityTypeConfiguration<ArgumentEntity> {
    [Required]
    public uint Index { get; set; }
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

    public void Configure(EntityTypeBuilder<ArgumentEntity> builder)
        => builder.Property(p => p.Choices)
                  .HasConversion(o => StringArrayConversion.ConvertToString(o),
                                 s => StringArrayConversion.ConvertToArray(s),
                                 new StringArrayComparer());

    public ArgumentData ToDto()
        => new() {
            Name = Name,
            Description = Description,
            Type = Type,
            Choices = Choices?.ToList() ?? [],
            IsRequired = IsRequired,
        };
}
