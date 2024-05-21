namespace DotNetToolbox.Data.Strategies.Key;

public sealed class InMemoryDateTimeKeyHandler(string contextKey, IDateTimeProvider? dateTime = null)
    : InMemoryKeyHandler<DateTimeOffset>(contextKey, EqualityComparer<DateTimeOffset>.Default) {
    private readonly IDateTimeProvider _dateTime = dateTime ?? DateTimeProvider.Default;

    protected override DateTimeOffset GenerateNewKey(DateTimeOffset lastUsedKey)
        => _dateTime.UtcNow;
}
