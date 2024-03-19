using Sophia.Models.Tools;

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
                  .HasConversion(o => ConvertChoicesToString(o),
                                 s => ConvertStringToChoices(s),
                                 new OptionsComparer());

    private static string[] ConvertStringToChoices(string? s)
        => s == null ? [] : s.Split('|');

    private static string? ConvertChoicesToString(string[] choices)
        => choices.Length == 0 ? null : string.Join('|', choices);

    public ArgumentData ToDto()
        => new() {
            Name = Name,
            Description = Description,
            Type = Type,
            Choices = Choices?.ToList() ?? [],
            IsRequired = IsRequired,
        };

    private class OptionComparer()
        : ValueComparer<string>((a, b) => a != null && a.Equals(b, StringComparison.InvariantCultureIgnoreCase),
                                s => s.GetHashCode());

    private class OptionsComparer()
        : ValueComparer<string[]>((a, b) => a != null && b != null && a.SequenceEqual(b, new OptionComparer()),
                                  s => s.Aggregate(0, HashCode.Combine));
}
