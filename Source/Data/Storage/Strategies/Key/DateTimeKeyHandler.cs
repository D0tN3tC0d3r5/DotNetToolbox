using DotNetToolbox.Environment;

namespace DotNetToolbox.Data.Strategies.Key;

public sealed class DateTimeKeyHandler(IDateTimeProvider dateTime)
    : KeyHandler<DateTimeOffset>(EqualityComparer<DateTimeOffset>.Default) {
    private readonly IDateTimeProvider _dateTime = dateTime ?? DateTimeProvider.Default;

    public override DateTimeOffset GetNext(string contextKey, DateTimeOffset proposedKey) {
        lock (Lock)
            return proposedKey == DateTimeOffset.MinValue ? _dateTime.UtcNow : proposedKey;
    }
}
