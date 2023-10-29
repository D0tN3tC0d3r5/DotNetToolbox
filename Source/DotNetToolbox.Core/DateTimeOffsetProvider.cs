namespace System;

[ExcludeFromCodeCoverage(Justification = "Thin wrapper for OS functionality.")]
// ReSharper disable once ClassWithVirtualMembersNeverInherited.Global - Used for testing.
public class DateTimeOffsetProvider {

    public virtual DateTimeOffset Now => DateTimeOffset.Now;
    public virtual DateOnly Today => DateOnly.FromDateTime(DateTimeOffset.Now.DateTime);
    public virtual TimeOnly TimeOfDay => TimeOnly.FromTimeSpan(DateTimeOffset.Now.TimeOfDay);
    public virtual DateTimeOffset Minimum => DateTimeOffset.MinValue;
    public virtual DateTimeOffset Maximum => DateTimeOffset.MaxValue;
    public virtual DateTimeOffset UnixEpoch => DateTimeOffset.UnixEpoch;
}
