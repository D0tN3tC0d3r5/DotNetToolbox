namespace System.Validation.Builder;

public static class ValidatorFactory {
    public static IConnector<CollectionValidator<TSubject>> Create<TSubject>(ICollection<TSubject?>? subject, string source, ValidatorMode mode = ValidatorMode.And) {
        var validator = new CollectionValidator<TSubject>(subject, source, mode);
        return validator.IsNotNull();
    }

    public static IConnector<CollectionValidator<TSubject>> Create<TSubject>(ICollection<TSubject?>? subject, string source, Func<TSubject?, ITerminator> itemIsValid, ValidatorMode mode = ValidatorMode.And) {
        var validator = new CollectionValidator<TSubject>(subject, source, mode);
        return validator.IsNotNull().And().Each(itemIsValid);
    }

    public static IConnector<DateTimeValidator> Create(bool allowNull, DateTime? subject, string source, ValidatorMode mode = ValidatorMode.And) {
        var validator = new DateTimeValidator(subject, source, mode);
        return allowNull ? validator.Connector : validator.IsNotNull();
    }

    public static IConnector<DecimalValidator> Create(bool allowNull, decimal? subject, string source, ValidatorMode mode = ValidatorMode.And) {
        var validator = new DecimalValidator(subject, source, mode);
        return allowNull ? validator.Connector : validator.IsNotNull();
    }

    public static IConnector<DictionaryValidator<TSubjectKey, TSubjectValue>> Create<TSubjectKey, TSubjectValue>(IDictionary<TSubjectKey, TSubjectValue?>? subject, string source, ValidatorMode mode = ValidatorMode.And)
        where TSubjectKey : notnull {
        var validator = new DictionaryValidator<TSubjectKey, TSubjectValue>(subject, source, mode);
        return validator.IsNotNull();
    }

    public static IConnector<DictionaryValidator<TSubjectKey, TSubjectValue>> Create<TSubjectKey, TSubjectValue>(IDictionary<TSubjectKey, TSubjectValue?>? subject, string source, Func<TSubjectValue?, ITerminator> validateValue, ValidatorMode mode = ValidatorMode.And)
        where TSubjectKey : notnull {
        var validator = new DictionaryValidator<TSubjectKey, TSubjectValue>(subject, source, mode);
        return validator.IsNotNull().And().Each(validateValue);
    }

    public static IConnector<IntegerValidator> Create(bool allowNull, int? subject, string source, ValidatorMode mode = ValidatorMode.And) {
        var validator = new IntegerValidator(subject, source, mode);
        return allowNull ? validator.Connector : validator.IsNotNull();
    }

    public static IConnector<ObjectValidator> Create(bool allowNull, object? subject, string source, ValidatorMode mode = ValidatorMode.And) {
        var validator = new ObjectValidator(subject, source, mode);
        return allowNull ? validator.Connector : validator.IsNotNull();
    }

    public static IConnector<StringValidator> Create(bool allowNull, string? subject, string source, ValidatorMode mode = ValidatorMode.And) {
        var validator = new StringValidator(subject, source, mode);
        return allowNull ? validator.Connector : validator.IsNotNull();
    }

    public static IConnector<TypeValidator> Create(Type? subject, string source, ValidatorMode mode = ValidatorMode.And) {
        var validator = new TypeValidator(subject, source, mode);
        return validator.IsNotNull();
    }

    public static IConnector<ValidatableValidator> Create(bool allowNull, IValidatable? subject, string source, ValidatorMode mode = ValidatorMode.And) {
        var validator = new ValidatableValidator(subject, source, mode);
        return allowNull ? validator.Connector : validator.IsNotNull();
    }
}
