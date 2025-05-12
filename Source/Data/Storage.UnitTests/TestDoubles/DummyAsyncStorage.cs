
namespace DotNetToolbox.Data.TestDoubles;

internal sealed class DummyAsyncStorage()
    : AsyncStorage<DummyAsyncStorage, TestEntity, uint>("TestEntity", InMemoryAsyncKeyGenerator<uint>.Instance, []) {
    public override Task<Result> AddAsync(TestEntity newItem, IMap? validationContext = null, CancellationToken ct = default) => throw new NotImplementedException();
    public override Task<Result> AddManyAsync(IEnumerable<TestEntity> newItems, IMap? validationContext = null, CancellationToken ct = default) => throw new NotImplementedException();
    public override Task<Result> AddOrUpdateAsync(TestEntity updatedItem, IMap? validationContext = null, CancellationToken ct = default) => throw new NotImplementedException();
    public override Task<Result> AddOrUpdateAsync(Expression<Func<TestEntity, bool>> predicate, TestEntity updatedItem, IMap? validationContext = null, CancellationToken ct = default) => throw new NotImplementedException();
    public override Task<Result> AddOrUpdateManyAsync(IEnumerable<TestEntity> updatedItems, IMap? validationContext = null, CancellationToken ct = default) => throw new NotImplementedException();
    public override Task<Result> AddOrUpdateManyAsync(Expression<Func<TestEntity, bool>> predicate, IEnumerable<TestEntity> updatedItems, IMap? validationContext = null, CancellationToken ct = default) => throw new NotImplementedException();
    public override Task<Result> ClearAsync(CancellationToken ct = default) => throw new NotImplementedException();
    public override Task<Result<TestEntity>> CreateAsync(Func<TestEntity, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default) => throw new NotImplementedException();
    public override ValueTask<TestEntity?> FindAsync(Expression<Func<TestEntity, bool>> predicate, CancellationToken ct = default) => throw new NotImplementedException();
    public override ValueTask<TestEntity?> FindByKeyAsync(uint key, CancellationToken ct = default) => throw new NotImplementedException();
    public override ValueTask<TestEntity[]> GetAllAsync(Expression<Func<TestEntity, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null, CancellationToken ct = default) => throw new NotImplementedException();
    public override ValueTask<Chunk<TestEntity>> GetChunkAsync(Expression<Func<TestEntity, bool>>? isChunkStart = null, uint blockSize = 20, Expression<Func<TestEntity, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null, CancellationToken ct = default) => throw new NotImplementedException();
    public override ValueTask<Page<TestEntity>> GetPageAsync(uint pageIndex = 0, uint pageSize = 20, Expression<Func<TestEntity, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null, CancellationToken ct = default) => throw new NotImplementedException();
    public override Task<Result> PatchAsync(uint key, Func<TestEntity, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default) => throw new NotImplementedException();
    public override Task<Result> PatchAsync(Expression<Func<TestEntity, bool>> predicate, Action<TestEntity> setItem, IMap? validationContext = null, CancellationToken ct = default) => throw new NotImplementedException();
    public override Task<Result> PatchAsync(Expression<Func<TestEntity, bool>> predicate, Func<TestEntity, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default) => throw new NotImplementedException();
    public override Task<Result> PatchManyAsync(IEnumerable<uint> keys, Action<TestEntity> setItem, IMap? validationContext = null, CancellationToken ct = default) => throw new NotImplementedException();
    public override Task<Result> PatchManyAsync(IEnumerable<uint> keys, Func<TestEntity, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default) => throw new NotImplementedException();
    public override Task<Result> PatchManyAsync(Expression<Func<TestEntity, bool>> predicate, Action<TestEntity> setItem, IMap? validationContext = null, CancellationToken ct = default) => throw new NotImplementedException();
    public override Task<Result> PatchManyAsync(Expression<Func<TestEntity, bool>> predicate, Func<TestEntity, CancellationToken, Task> setItem, IMap? validationContext = null, CancellationToken ct = default) => throw new NotImplementedException();
    public override Task<Result> RemoveAsync(uint key, CancellationToken ct = default) => throw new NotImplementedException();
    public override Task<Result> RemoveAsync(Expression<Func<TestEntity, bool>> predicate, CancellationToken ct = default) => throw new NotImplementedException();
    public override Task<Result> RemoveManyAsync(IEnumerable<uint> keys, CancellationToken ct = default) => throw new NotImplementedException();
    public override Task<Result> RemoveManyAsync(Expression<Func<TestEntity, bool>> predicate, CancellationToken ct = default) => throw new NotImplementedException();
    public override Task<Result> SeedAsync(IEnumerable<TestEntity> seed, bool preserveContent = false, IMap? validationContext = null, CancellationToken ct = default) => throw new NotImplementedException();
    public override Task<Result> UpdateAsync(TestEntity updatedItem, IMap? validationContext = null, CancellationToken ct = default) => throw new NotImplementedException();
    public override Task<Result> UpdateAsync(Expression<Func<TestEntity, bool>> predicate, TestEntity updatedItem, IMap? validationContext = null, CancellationToken ct = default) => throw new NotImplementedException();
    public override Task<Result> UpdateManyAsync(IEnumerable<TestEntity> updatedItems, IMap? validationContext = null, CancellationToken ct = default) => throw new NotImplementedException();
}
