using Sophia.Models.Worlds;

namespace Sophia.WebApp.Data.World;

[Owned]
public class InformationEntity {
    [MaxLength(100)]
    public string? Value { get; set; }
    [MaxLength(1000)]
    public string ValueTemplate { get; set; } = string.Empty;
    [MaxLength(1000)]
    public string DefaultText { get; set; } = string.Empty;

    public InformationData ToDto()
        => new() {
                     DefaultText = DefaultText,
                     Value = Value,
                     ValueTemplate = ValueTemplate,
                 };
}
