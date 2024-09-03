namespace AI.Sample.Providers.Repositories;

public class ProviderEntity
    : Entity<ProviderEntity, uint> {
    public string Name { get; set; } = string.Empty;

    public string NormalizedName => NormalizeName(Name);

    public override Result Validate(IContext? context = null) {
        var result = base.Validate(context);
        if (string.IsNullOrWhiteSpace(Name)) result += new ValidationError("Name is required.", nameof(Name));
        return result;
    }

    private static string NormalizeName(string name) {
        var normalized = name.ToLowerInvariant();
        normalized = normalized.Replace(" ", "-");
        normalized = RemovePunctuation(normalized);
        return normalized;
    }

    private static string RemovePunctuation(string input) {
        var sb = new StringBuilder();
        foreach (var c in input.Where(static i => !char.IsPunctuation(i)))
            sb.Append(c);
        return sb.ToString();
    }
}
