namespace System.Validation.Builder;

public interface IBinaryConnector<out TValidator>
    where TValidator : IValidator {
    TValidator And();
    TValidator Or();
    TValidator AndNot();
    TValidator OrNot();
}