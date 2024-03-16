using Microsoft.EntityFrameworkCore.ChangeTracking;

using Sophia.Models.Skills;

namespace Sophia.WebApp.Data.World;

[Owned]
[EntityTypeConfiguration(typeof(ArgumentEntity))]
public class ArgumentEntity
    : IEntityTypeConfiguration<ArgumentEntity> {
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(20)]
    public ArgumentType Type { get; set; }
    [MaxLength(2000)]
    public string? Description { get; set; }
    [MaxLength(1000)]
    public string[]? Options { get; set; }
    public bool IsRequired { get; set; }
    public void Configure(EntityTypeBuilder<ArgumentEntity> builder)
        => builder.Property(p => p.Options)
                  .HasConversion(o => o == null ? null : string.Join('|', o),
                                 s => s == null ? null : s.Split('|', StringSplitOptions.None),
                                 new OptionsComparer());

    public ArgumentData ToDto()
        => new() {
                     Name = Name,
                     Description = Description,
                     Type = Type,
                     Options = Options,
                     IsRequired = IsRequired,
                 };

    private class OptionComparer()
        : ValueComparer<string>((a, b) => a != null && a.Equals(b, StringComparison.InvariantCultureIgnoreCase),
                                s => s.GetHashCode());

    private class OptionsComparer()
        : ValueComparer<string[]>((a, b) => a != null && b != null && a.SequenceEqual(b, new OptionComparer()),
                                  s => s.Aggregate(0, HashCode.Combine));
}
