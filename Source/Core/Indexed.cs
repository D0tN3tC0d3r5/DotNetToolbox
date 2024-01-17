namespace DotNetToolbox;

public readonly struct Indexed<TValue>(int index, TValue? value) {
    public int Index { get; } = index;
    public TValue? Value { get; } = value;
}
