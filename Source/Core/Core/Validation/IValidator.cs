namespace DotNetToolbox.Validation;

public interface IValidator<in TValue> :
    IValidator<TValue, IMap>;

public interface IValidator<in TValue, in TContext>
    where TContext : class {
    bool IsValid(TValue value, TContext? context = null);
}
