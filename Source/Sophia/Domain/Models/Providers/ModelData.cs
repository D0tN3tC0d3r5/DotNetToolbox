using DotNetToolbox.AI.Providers;

namespace Sophia.Models.Providers;

public class ModelData
    : IEntity<string> {
    [Required]
    [MaxLength(50)]
    public string Id { get; set; } = default!;
    [Required]
    public ProviderData Provider { get; set; } = default!;
    [MaxLength(50)]
    public string Name { get; set; } = default!;

    public Model ToModel()
        => new() {
            Id = Id,
            Name = Name,
            Provider = Provider.Name,
        };
}
