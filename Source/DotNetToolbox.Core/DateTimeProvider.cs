namespace System;

[ExcludeFromCodeCoverage(Justification = "Thin wrapper for OS functionality.")]
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global - Used for testing.
public class DateTimeProvider {

    public virtual DateTime LocalNow => DateTime.Now;
    public virtual DateOnly LocalToday => DateOnly.FromDateTime(DateTime.Now);
    public virtual TimeOnly LocalTimeOfDay => TimeOnly.FromDateTime(DateTime.Now);

    public virtual DateTime UtcNow => DateTime.UtcNow;
    public virtual DateOnly UtcToday => DateOnly.FromDateTime(DateTime.UtcNow);
    public virtual TimeOnly UtcTimeOfDay => TimeOnly.FromDateTime(DateTime.UtcNow);

    public virtual DateTime Minimum => DateTime.MinValue;
    public virtual DateTime Maximum => DateTime.MaxValue;
    public virtual DateTime UnixEpoch => DateTime.UnixEpoch;
}