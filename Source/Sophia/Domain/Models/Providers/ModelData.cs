namespace Sophia.Models.Providers;

public class ModelData {
    [Required]
    [MaxLength(50)]
    public string Key { get; set; } = string.Empty;
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
}
