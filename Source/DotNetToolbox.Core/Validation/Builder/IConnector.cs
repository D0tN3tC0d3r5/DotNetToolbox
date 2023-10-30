namespace System.Validation.Builder;

public interface IConnector<out TValidator>
    : ITerminator,
      IBinaryConnector<TValidator>,
      IBinaryOperator<TValidator>
    where TValidator : IValidator {
}