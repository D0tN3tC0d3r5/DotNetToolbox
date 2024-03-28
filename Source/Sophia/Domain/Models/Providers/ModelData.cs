namespace Sophia.Models.Providers;

public class ModelData {
    [Required]
    [MaxLength(50)]
    public string Id { get; set; } = string.Empty;
    [MaxLength(50)]
    public string? Name { get; set; }
}
