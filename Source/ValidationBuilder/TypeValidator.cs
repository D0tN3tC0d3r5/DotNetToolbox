namespace DotNetToolbox.ValidationBuilder;

public class TypeValidator : Validator<Type?>, ITypeValidator {
    private readonly ValidationCommandFactory _commandFactory;

    public TypeValidator(Type? subject, string source, ValidatorMode mode = And)
        : base(subject, source, mode) {
        Connector1 = new Connector<Type?, TypeValidator>(this);
        _commandFactory = ValidationCommandFactory.For(typeof(Type), Source);
    }

    public IConnector<TypeValidator> Connector1 { get; }

    public IConnector<TypeValidator> IsNull() {
        var validator = _commandFactory.Create(nameof(IsNull));
        ValidateWith(validator);
        return Connector1;
    }

    public IConnector<TypeValidator> IsNotNull() {
        Negate();
        return IsNull();
    }

    public IConnector<TypeValidator> IsEqualTo<TType>() {
        var validator = _commandFactory.Create(nameof(IsEqualTo), typeof(TType));
        ValidateWith(validator);
        return Connector1;
    }
}