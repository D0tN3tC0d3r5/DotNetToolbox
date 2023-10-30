namespace System.Validation;

public static class ValidationExtensions {
    public static IConnector<ObjectValidator> IsRequired(this object? subject, [CallerArgumentExpression(nameof(subject))] string? source = null)
        => Create(allowNull: false, subject, source!);
    public static IConnector<ObjectValidator> IsOptional(this object? subject, [CallerArgumentExpression(nameof(subject))] string? source = null)
        => Create(allowNull: true, subject, source!);

    public static IConnector<ValidatableValidator> IsOptional(this IValidatable? subject, [CallerArgumentExpression(nameof(subject))] string? source = null)
        => Create(allowNull: true, subject, source!);
    public static IConnector<ValidatableValidator> IsRequired(this IValidatable? subject, [CallerArgumentExpression(nameof(subject))] string? source = null)
        => Create(allowNull: false, subject, source!);

    public static IConnector<TypeValidator> Is(this Type? subject, [CallerArgumentExpression(nameof(subject))] string? source = null)
        => Create(subject, source!);

    public static IConnector<IntegerValidator> Is(this int subject, [CallerArgumentExpression(nameof(subject))] string? source = null)
        => Create(allowNull: false, subject, source!);
    public static IConnector<IntegerValidator> IsOptional(this int? subject, [CallerArgumentExpression(nameof(subject))] string? source = null)
        => Create(allowNull: true, subject, source!);
    public static IConnector<IntegerValidator> IsRequired(this int? subject, [CallerArgumentExpression(nameof(subject))] string? source = null)
        => Create(allowNull: false, subject, source!);

    public static IConnector<DecimalValidator> Is(this decimal subject, [CallerArgumentExpression(nameof(subject))] string? source = null)
        => Create(allowNull: false, subject, source!);
    public static IConnector<DecimalValidator> IsOptional(this decimal? subject, [CallerArgumentExpression(nameof(subject))] string? source = null)
        => Create(allowNull: true, subject, source!);
    public static IConnector<DecimalValidator> IsRequired(this decimal? subject, [CallerArgumentExpression(nameof(subject))] string? source = null)
        => Create(allowNull: false, subject, source!);

    public static IConnector<DateTimeValidator> Is(this DateTime subject, [CallerArgumentExpression(nameof(subject))] string? source = null)
        => Create(allowNull: false, subject, source!);
    public static IConnector<DateTimeValidator> IsOptional(this DateTime? subject, [CallerArgumentExpression(nameof(subject))] string? source = null)
        => Create(allowNull: true, subject, source!);
    public static IConnector<DateTimeValidator> IsRequired(this DateTime? subject, [CallerArgumentExpression(nameof(subject))] string? source = null)
        => Create(allowNull: false, subject, source!);

    public static IConnector<StringValidator> IsOptional(this string? subject, [CallerArgumentExpression(nameof(subject))] string? source = null)
        => Create(allowNull: true, subject, source!);
    public static IConnector<StringValidator> IsRequired(this string? subject, [CallerArgumentExpression(nameof(subject))] string? source = null)
        => Create(allowNull: false, subject, source!);

    public static IConnector<CollectionValidator<TItem>> Is<TItem>(this ICollection<TItem?> subject, [CallerArgumentExpression(nameof(subject))] string? source = null)
        => Create(subject, source!);
    public static IConnector<CollectionValidator<TItem>> CheckIfEach<TItem>(this ICollection<TItem?> subject, Func<TItem?, ITerminator> itemIsValid, [CallerArgumentExpression(nameof(subject))] string? source = null)
        => Create(subject, source!, itemIsValid);

    public static IConnector<DictionaryValidator<TKey, TValue>> Is<TKey, TValue>(this IDictionary<TKey, TValue?>? subject, [CallerArgumentExpression(nameof(subject))] string? source = null)
        where TKey : notnull
        => Create(subject, source!);
    public static IConnector<DictionaryValidator<TKey, TValue>> CheckIfEach<TKey, TValue>(this IDictionary<TKey, TValue?>? subject, Func<TValue?, ITerminator> validate, [CallerArgumentExpression(nameof(subject))] string? source = null)
        where TKey : notnull
        => Create(subject, source!, validate);
}
