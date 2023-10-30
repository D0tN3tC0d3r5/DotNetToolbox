namespace System.Validation.Builder;

public interface IDictionaryValidator<TKey, TValue> : IValidator
    where TKey : notnull {
    IConnector<DictionaryValidator<TKey, TValue>> IsNull();
    IConnector<DictionaryValidator<TKey, TValue>> IsNotNull();
    IConnector<DictionaryValidator<TKey, TValue>> IsNotEmpty();
    IConnector<DictionaryValidator<TKey, TValue>> HasAtMost(int size);
    IConnector<DictionaryValidator<TKey, TValue>> HasAtLeast(int size);
    IConnector<DictionaryValidator<TKey, TValue>> Has(int size);
    IConnector<DictionaryValidator<TKey, TValue>> ContainsKey(TKey key);

    IConnector<DictionaryValidator<TKey, TValue>> Each(Func<TValue?, ITerminator> validateUsing);
}