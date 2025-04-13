namespace DotNetToolbox.Sequences;

public class GuidSequence
    : Sequence<Guid> {
    protected override bool TryGetNext(out Guid next) {
        next = Guid.CreateVersion7();
        return true;
    }
}
