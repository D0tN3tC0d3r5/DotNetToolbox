namespace DotNetToolbox.Data.Strategies.Key;

public sealed class IntKeyHandler()
    : KeyHandler<int>(EqualityComparer<int>.Default) {
    public override int GetNext(string contextKey, int proposedKey) {
        return (int)Context.AddOrUpdate(GetKey(contextKey), _ => SetInitialValue(proposedKey), (_, v) => UpdateValue((int)v!, proposedKey))!;
        static string GetKey(string? key) => key ?? nameof(Int32);
        static int SetInitialValue(int proposed) => int.Max(proposed, 1);
        static int UpdateValue(int value, int proposed) => int.Max(proposed, value + 1);
    }
}
