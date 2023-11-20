namespace DotNetToolbox.CommandLineBuilder;

public interface IHasValue {
    Type ValueType { get; }
}

public interface IHasValue<out TValue> : IHasValue {
    TValue Value { get; }
}

public interface IHasValues : IHasValue { }

public interface IHasValues<out TValue> : IHasValues {
    public IReadOnlyList<TValue> Values { get; }
}
