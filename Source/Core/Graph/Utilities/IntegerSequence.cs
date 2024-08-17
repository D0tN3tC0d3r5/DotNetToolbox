namespace DotNetToolbox.Graph.Utilities;

public sealed class IntegerSequence : IntegerSequence<IntegerSequence>;

public class IntegerSequence<TSequence>(int first)
        : Sequence<TSequence, int>(first)
    where TSequence : IntegerSequence<TSequence> {
    public IntegerSequence()
        : this(0) {
    }

    protected sealed override void SetNext(int value) {
        if (value <= First) {
            throw new ArgumentOutOfRangeException(nameof(value), $"The next value must be greater than '{First}'.");
        }
        Current = value - 1;
    }

    protected sealed override bool TryGenerateNext([NotNullWhen(true)] out int next) {
        try {
            next = Current + 1;
            return true;
        }
        catch {
            next = Current;
            return false;
        }
    }
}
