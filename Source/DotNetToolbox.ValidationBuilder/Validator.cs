namespace DotNetToolbox.ValidationBuilder;

public abstract class Validator(string source, ValidatorMode mode = And) : IValidator {
    public ValidatorMode Mode { get; private set; } = mode;
    public string Source { get; } = source;
    public Result Result { get; private set; } = Result.Success();

    public Validator SetMode(ValidatorMode mode) {
        Mode = mode;
        return this;
    }

    public void Negate() => Mode ^= Not;

    public void ClearErrors() => Result = Result.Success();

    public void AddError(ValidationError error) => Result += error;

    public void AddErrors(IEnumerable<ValidationError> errors) => Result += errors.ToArray();
}

public abstract class Validator<TSubject>(TSubject? subject, string source, ValidatorMode mode = And) : Validator(source, mode) {
    public TSubject? Subject { get; } = subject;

    protected void ValidateWith(IValidationCommand validator) {
        var rightResult = Mode.Has(Not)
            ? validator.Negate(Subject)
            : validator.Validate(Subject);
        if (Mode.Has(Or) && (rightResult.IsSuccess || Result.IsSuccess)) {
            ClearErrors();
            return;
        }

        AddErrors(rightResult.Errors);
    }
}
