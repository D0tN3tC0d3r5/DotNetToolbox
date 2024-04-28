namespace DotNetToolbox.Data.Strategies;

// ReSharper disable once UnusedTypeParameter
public interface IAsyncRepositoryStrategy<TItem>
    : IAsyncRepository<TItem>
    , IRepositoryStrategy<TItem> {
}
