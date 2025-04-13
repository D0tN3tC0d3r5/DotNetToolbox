namespace DotNetToolbox.Validation;

public interface IValidatable : IValidatable<IMap>;

public interface IValidatable<in TContext>
    where TContext : class {
    Result Validate(TContext? context = null);
}
