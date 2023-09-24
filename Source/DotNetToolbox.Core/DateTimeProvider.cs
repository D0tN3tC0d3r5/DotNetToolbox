namespace DotNetToolbox;

[ExcludeFromCodeCoverage(Justification = "Thin wrapper for OS functionality.")]
public class DateTimeProvider
{
    public virtual DateTimeOffset NowWithTimeZone => DateTimeOffset.Now;

    public virtual DateTime LocalNow => DateTime.Now;
    public virtual DateOnly LocalToday => DateOnly.FromDateTime(DateTime.Now);
    public virtual TimeOnly LocalTimeOfDay => TimeOnly.FromDateTime(DateTime.Now);

    public virtual DateTime UtcNow => DateTime.UtcNow;
    public virtual DateOnly UtcToday => DateOnly.FromDateTime(DateTime.UtcNow);
    public virtual TimeOnly UtcTimeOfDay => TimeOnly.FromDateTime(DateTime.UtcNow);

    public virtual DateTime MinimumWithoutTimeZone => DateTime.MaxValue;
    public virtual DateTime MaximumWithoutTimeZone => DateTime.MinValue;
    public virtual DateTimeOffset MinimumWithTimeZone => DateTimeOffset.MaxValue;
    public virtual DateTimeOffset MaximumWithTimeZone => DateTimeOffset.MinValue;
}
