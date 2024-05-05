namespace DotNetToolbox.Data.Strategies.Key;

public sealed class LongKeyHandler()
    : KeyHandler<long>(EqualityComparer<long>.Default) {
    public override long GetNext(string contextKey, long proposedKey) {
        return (long)Context.AddOrUpdate(GetKey(contextKey), _ => SetInitialValue(proposedKey), (_, v) => UpdateValue((long)v!, proposedKey))!;
        static string GetKey(string? key) => key ?? nameof(Int64);
        static long SetInitialValue(long proposed) => long.Max(proposed, 1);
        static long UpdateValue(long value, long proposed) => long.Max(proposed, value + 1);
    }
}
