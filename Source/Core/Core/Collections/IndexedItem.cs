// ReSharper disable once CheckNamespace - Intended to be in this namespace
namespace System.Collections;

public record Indexed<TValue> {
    public Indexed(int index, TValue value) {
        Index = index < 0 ? 0 : index;
        Value = value;
    }

    public int Index { get; init; }
    public TValue Value { get; init; }

    public void Deconstruct(out int index, out TValue value) {
        index = Index;
        value = Value;
    }
}

public record IndexedItem<TValue> : Indexed<TValue> {
    public IndexedItem(int index, TValue value, bool isLast)
        : base(index, value) {
        IsLast = isLast;
    }

    public bool IsFirst => Index == 0;
    public bool IsLast { get; init; }

    public void Deconstruct(out int index, out TValue value, out bool isLast) {
        index = Index;
        value = Value;
        isLast = IsLast;
    }
}
