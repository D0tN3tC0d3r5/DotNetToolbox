namespace DotNetToolbox.ConsoleApplication;

public record ApplicationOptions<TOptions>
    : IApplicationOptions,
      INamedOptions<TOptions>
    where TOptions : ApplicationOptions<TOptions> {
    public static string SectionName => GetSectionName();

    protected static string GetSectionName() {
        var typeName = typeof(TOptions).Name;
        return typeName.EndsWith("Options") ? typeName.Remove(7) : typeName;
    }

    [Required]
    public string Name { get; init; } = default!;
    public string? Environment { get; init; }
    public string Version { get; init; } = "1.0.0";
    public string? Description { get; init; }
}

public record ApplicationOptions
    : ApplicationOptions<ApplicationOptions> {
}
