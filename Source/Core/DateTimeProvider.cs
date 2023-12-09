namespace DotNetToolbox;

[ExcludeFromCodeCoverage(Justification = "Thin wrapper for OS functionality.")]
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global - Used for testing.
public class DateTimeProvider {
    public static readonly DateTimeProvider Default = new();

    public virtual DateTimeOffset LocalNow => DateTimeOffset.Now;
    public virtual DateOnly LocalToday => DateOnly.FromDateTime(LocalNow.DateTime);
    public virtual TimeOnly LocalTimeOfDay => TimeOnly.FromDateTime(LocalNow.DateTime);

    public virtual DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    public virtual DateOnly UtcToday => DateOnly.FromDateTime(UtcNow.DateTime);
    public virtual TimeOnly UtcTimeOfDay => TimeOnly.FromDateTime(UtcNow.DateTime);

    public virtual DateTime Minimum => DateTime.MinValue;
    public virtual DateTime Maximum => DateTime.MaxValue;
    public virtual DateTime UnixEpoch => DateTime.UnixEpoch;

    public virtual DateTime Parse(string s) => DateTime.Parse(s);
    public virtual bool TryParse(string s, out DateTime result)
        => DateTime.TryParse(s, out result);

    public virtual DateTime ParseExact(string s, [StringSyntax(DateTimeFormat)] string format)
        => DateTime.ParseExact(s, format, null, DateTimeStyles.None);
    public virtual bool TryParseExact(
        string s,
        [StringSyntax(DateTimeFormat)] string format,
        out DateTime result)
        => DateTime.TryParseExact(s, format, null, DateTimeStyles.None, out result);
}
