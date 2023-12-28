namespace DotNetToolbox.Options;

public interface INamedOptions<TOptions> {
    public static virtual string SectionName { get; } = typeof(TOptions).Name;
}
