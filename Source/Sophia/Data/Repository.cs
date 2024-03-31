using System.Diagnostics.CodeAnalysis;

using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace Sophia.Data;

public abstract class Repository<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes)]TDomainModel>(DataContext dataContext)
    : IRepository<TDomainModel>
    where TDomainModel : class {
    internal const DynamicallyAccessedMemberTypes DynamicallyAccessedMemberTypes =
        PublicConstructors | NonPublicConstructors
      | PublicProperties | NonPublicProperties
      | PublicFields | NonPublicFields
      | Interfaces;

    protected DataContext DataContext { get; } = dataContext;
    public virtual IAsyncEnumerable<TDomainModel> AsAsyncEnumerable() => (IAsyncEnumerable<TDomainModel>)this;
    public virtual IQueryable<TDomainModel> AsQueryable() => this;
    public Type ElementType => typeof(TDomainModel);
    public abstract IAsyncEnumerator<TDomainModel> GetAsyncEnumerator(CancellationToken ct = default);
    public abstract IEnumerator<TDomainModel> GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    public abstract Expression Expression { get; }
    public abstract IQueryProvider Provider { get; }

    public abstract Task Add(TDomainModel input, CancellationToken ct = default);
    public abstract Task Update(TDomainModel input, CancellationToken cancellationToken = default);
    public abstract Task Remove(TDomainModel input, CancellationToken cancellationToken = default);
}
