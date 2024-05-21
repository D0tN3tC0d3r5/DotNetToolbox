namespace DotNetToolbox.ValidationBuilder;

public interface IBinaryConnector<out TValidator>
    where TValidator : IValidator {
    TValidator And();
    TValidator Or();
    TValidator AndNot();
    TValidator OrNot();
}