namespace DotNetToolbox.Options;

public interface INamedOptions<TOptions>
    where TOptions : INamedOptions<TOptions> {
    public static abstract string SectionName { get; }
}
