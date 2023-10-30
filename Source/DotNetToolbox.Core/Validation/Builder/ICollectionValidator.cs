namespace System.Validation.Builder;

public interface ICollectionValidator<TItem> : IValidator {
    IConnector<CollectionValidator<TItem>> IsNull();
    IConnector<CollectionValidator<TItem>> IsNotNull();
    IConnector<CollectionValidator<TItem>> IsEmpty();
    IConnector<CollectionValidator<TItem>> IsNotEmpty();
    IConnector<CollectionValidator<TItem>> HasAtMost(int size);
    IConnector<CollectionValidator<TItem>> HasAtLeast(int size);
    IConnector<CollectionValidator<TItem>> Has(int size);
    IConnector<CollectionValidator<TItem>> Contains(TItem item);

    IConnector<CollectionValidator<TItem>> Each(Func<TItem?, ITerminator> validate);
}
