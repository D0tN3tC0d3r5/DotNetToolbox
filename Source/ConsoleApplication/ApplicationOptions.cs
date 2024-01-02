namespace DotNetToolbox.ConsoleApplication;

public record ApplicationOptions<TOptions>
    : IApplicationOptions
    where TOptions : ApplicationOptions<TOptions> {
    [Required]
    public string? Name { get; init; }
    public string? Environment { get; init; }
    public string? Version { get; init; }
    public string? Description { get; init; }
}

public record ApplicationOptions
    : ApplicationOptions<ApplicationOptions> {
}
