namespace Sophia.Data.Providers;

[Table("Models")]
public class ModelEntity
    : IEntity<string> {

    [Required]
    [MaxLength(50)]
    public string Id { get; set; } = default!;
    public int ProviderId { get; set; }
    public ProviderEntity Provider { get; set; } = default!;
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = default!;
}
