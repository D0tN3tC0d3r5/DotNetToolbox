namespace DotNetToolbox.Graph.Utilities;

public sealed class CountableSequence : CountableSequence<CountableSequence>;

public class CountableSequence<TSequence>(uint first)
        : Sequence<TSequence, uint>(first)
    where TSequence : CountableSequence<TSequence> {
    public CountableSequence()
        : this(0) {
    }

    protected override void SetNext(uint value) {
        if (value <= First) {
            throw new ArgumentOutOfRangeException(nameof(value), $"The next value must be greater than '{First}'.");
        }
        Current = value - 1u;
    }

    protected override bool TryGenerateNext([NotNullWhen(true)] out uint next) {
        try {
            next = Current + 1u;
            return true;
        }
        catch {
            next = Current;
            return false;
        }
    }
}
