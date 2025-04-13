namespace DotNetToolbox.Sequences;

public class DateTimeOffsetSequence
    : Sequence<DateTimeOffset> {
    protected override bool TryGetNext(out DateTimeOffset next) {
        next = DateTimeOffset.UtcNow;
        return true;
    }
}
