using Create = System.Create;

namespace DotNetToolbox.ValidationBuilder;

public class Connector<TSubject, TValidator>
    : IConnector<TValidator>
    where TValidator : Validator<TSubject> {
    private readonly TValidator _left;

    public Connector(TValidator left) {
        _left = left;
    }

    public Result Result => _left.Result;

    public TValidator And() => (TValidator)_left.SetMode(ValidatorMode.And);
    public TValidator Or() => (TValidator)_left.SetMode(ValidatorMode.Or);
    public TValidator AndNot() => (TValidator)_left.SetMode(ValidatorMode.AndNot);
    public TValidator OrNot() => (TValidator)_left.SetMode(ValidatorMode.OrNot);

    public TValidator And(Func<TValidator, ITerminator> validateRight)
        => ProcessAnd(validateRight, ValidatorMode.And);

    public TValidator AndNot(Func<TValidator, ITerminator> validateRight)
        => ProcessAnd(validateRight, ValidatorMode.AndNot);

    public TValidator Or(Func<TValidator, ITerminator> validateRight)
        => ProcessOr(validateRight, ValidatorMode.And);

    public TValidator OrNot(Func<TValidator, ITerminator> validateRight)
        => ProcessOr(validateRight, ValidatorMode.AndNot);

    private TValidator ProcessAnd(Func<TValidator, ITerminator> validateRight, ValidatorMode mode) {
        var rightValidator = Create.Instance<TValidator>(_left.Subject!, _left.Source, mode);
        var rightResult = validateRight(rightValidator).Result;
        _left.AddErrors(rightResult.Errors);
        return _left;
    }

    private TValidator ProcessOr(Func<TValidator, ITerminator> validateRight, ValidatorMode mode) {
        var rightValidator = Create.Instance<TValidator>(_left.Subject!, _left.Source, mode);
        var rightResult = validateRight(rightValidator).Result;
        if (_left.Result.IsInvalid && rightResult.IsInvalid)
            _left.AddErrors(rightResult.Errors);
        else
            _left.ClearErrors();
        return _left;
    }
}
