namespace DotNetToolbox.ValidationBuilder;

public class Connector<TSubject, TValidator>(TValidator left) : IConnector<TValidator>
    where TValidator : Validator<TSubject> {
    public Result Result => left.Result;

    public TValidator And() => (TValidator)left.SetMode(ValidatorMode.And);
    public TValidator Or() => (TValidator)left.SetMode(ValidatorMode.Or);
    public TValidator AndNot() => (TValidator)left.SetMode(ValidatorMode.AndNot);
    public TValidator OrNot() => (TValidator)left.SetMode(ValidatorMode.OrNot);

    public TValidator And(Func<TValidator, ITerminator> validateRight)
        => ProcessAnd(validateRight, ValidatorMode.And);

    public TValidator AndNot(Func<TValidator, ITerminator> validateRight)
        => ProcessAnd(validateRight, ValidatorMode.AndNot);

    public TValidator Or(Func<TValidator, ITerminator> validateRight)
        => ProcessOr(validateRight, ValidatorMode.And);

    public TValidator OrNot(Func<TValidator, ITerminator> validateRight)
        => ProcessOr(validateRight, ValidatorMode.AndNot);

    private TValidator ProcessAnd(Func<TValidator, ITerminator> validateRight, ValidatorMode mode) {
        var rightValidator = Create.Instance<TValidator>(left.Subject!, left.Source, mode);
        var rightResult = validateRight(rightValidator).Result;
        left.AddErrors(rightResult.Errors);
        return left;
    }

    private TValidator ProcessOr(Func<TValidator, ITerminator> validateRight, ValidatorMode mode) {
        var rightValidator = Create.Instance<TValidator>(left.Subject!, left.Source, mode);
        var rightResult = validateRight(rightValidator).Result;
        if (left.Result.IsInvalid && rightResult.IsInvalid)
            left.AddErrors(rightResult.Errors);
        else
            left.ClearErrors();
        return left;
    }
}
