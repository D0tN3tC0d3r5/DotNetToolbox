namespace DotNetToolbox;

public record Indexed<TValue>(uint Index, TValue Value);

public record IndexedItem<TValue>(uint Index, TValue Value, bool IsLast) : Indexed<TValue>(Index, Value) {
    public bool IsFirst => Index == 0;
}
