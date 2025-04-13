namespace DotNetToolbox.Sequences;

public class DateTimeValueSequence
    : Sequence<DateTime> {
    protected override bool TryGetNext(out DateTime next) {
        next = DateTime.UtcNow;
        return true;
    }
}
