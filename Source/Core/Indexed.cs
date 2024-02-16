namespace DotNetToolbox;

public readonly struct Indexed<TValue>(uint index, TValue value, bool isLast) {
    public uint Index { get; } = index;
    public TValue Value { get; } = value;
    public bool IsFirst => Index == 0;
    public bool IsLast => isLast;
}
