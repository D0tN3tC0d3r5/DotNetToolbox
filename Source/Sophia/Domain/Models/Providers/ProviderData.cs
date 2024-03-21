namespace Sophia.Models.Providers;

public class ProviderData {
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    public string? Authentication { get; set; }
    public List<ModelData> Models { get; set; } = [];

    // ReSharper disable ConvertIfStatementToSwitchStatement
    // ReSharper disable ConvertIfStatementToReturnStatement
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "<Pending>")]
    public string? ValidateModels() {
        if (Models.Count == 0) return "The model list cannot be empty.";
        if (Models.Any(i => string.IsNullOrWhiteSpace(i.Key))) return "The model key is required.";
        if (Models.Any(i => i.Key.Length > 50)) return "A model key cannot exceed 50 characters.";
        if (Models.Any(i => string.IsNullOrWhiteSpace(i.Name))) return "The model name is required.";
        if (Models.Any(i => i.Name.Length > 50)) return "A model name cannot exceed 50 characters.";
        if (Models.Count != Models.Distinct().Count()) return "Each model must be unique within the same tool.";
        return null;
    }
    // ReSharper restore ConvertIfStatementToReturnStatement
    // ReSharper restore ConvertIfStatementToSwitchStatement
}
