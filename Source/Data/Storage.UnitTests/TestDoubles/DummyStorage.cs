namespace DotNetToolbox.Data.TestDoubles;

internal sealed class DummyStorage()
    : Storage<DummyStorage, TestEntity, uint>("TestEntity", InMemoryKeyGenerator<uint>.Instance, []) {
    public override Result Add(TestEntity newItem, IMap? validationContext = null) => throw new NotImplementedException();
    public override Result AddMany(IEnumerable<TestEntity> newItems, IMap? validationContext = null) => throw new NotImplementedException();
    public override Result AddOrUpdate(TestEntity updatedItem, IMap? validationContext = null) => throw new NotImplementedException();
    public override Result AddOrUpdate(Expression<Func<TestEntity, bool>> predicate, TestEntity updatedItem, IMap? validationContext = null) => throw new NotImplementedException();
    public override Result AddOrUpdateMany(IEnumerable<TestEntity> updatedItems, IMap? validationContext = null) => throw new NotImplementedException();
    public override Result AddOrUpdateMany(Expression<Func<TestEntity, bool>> predicate, IEnumerable<TestEntity> items, IMap? validationContext = null) => throw new NotImplementedException();
    public override Result Clear() => throw new NotImplementedException();
    public override Result<TestEntity> Create(Action<TestEntity>? setItem = null, IMap? validationContext = null) => throw new NotImplementedException();
    public override TestEntity? Find(Expression<Func<TestEntity, bool>> predicate) => throw new NotImplementedException();
    public override TestEntity? FindByKey(uint key) => throw new NotImplementedException();
    public override TestEntity[] GetAll(Expression<Func<TestEntity, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null) => throw new NotImplementedException();
    public override Chunk<TestEntity> GetChunk(Expression<Func<TestEntity, bool>>? isChunkStart = null, uint blockSize = 20, Expression<Func<TestEntity, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null) => throw new NotImplementedException();
    public override Page<TestEntity> GetPage(uint pageIndex = 0, uint pageSize = 20, Expression<Func<TestEntity, bool>>? filterBy = null, HashSet<SortClause>? orderBy = null) => throw new NotImplementedException();
    public override Result Patch(uint key, Action<TestEntity> setItem, IMap? validationContext = null) => throw new NotImplementedException();
    public override Result Patch(Expression<Func<TestEntity, bool>> predicate, Action<TestEntity> setItem, IMap? validationContext = null) => throw new NotImplementedException();
    public override Result PatchMany(IEnumerable<uint> keys, Action<TestEntity> setItem, IMap? validationContext = null) => throw new NotImplementedException();
    public override Result PatchMany(Expression<Func<TestEntity, bool>> predicate, Action<TestEntity> setItem, IMap? validationContext = null) => throw new NotImplementedException();
    public override Result Remove(uint key) => throw new NotImplementedException();
    public override Result Remove(Expression<Func<TestEntity, bool>> predicate) => throw new NotImplementedException();
    public override Result RemoveMany(IEnumerable<uint> keys) => throw new NotImplementedException();
    public override Result RemoveMany(Expression<Func<TestEntity, bool>> predicate) => throw new NotImplementedException();
    public override Result Seed(IEnumerable<TestEntity> seed, bool preserveContent = false, IMap? validationContext = null) => throw new NotImplementedException();
    public override Result Update(TestEntity updatedItem, IMap? validationContext = null) => throw new NotImplementedException();
    public override Result Update(Expression<Func<TestEntity, bool>> predicate, TestEntity updatedItem, IMap? validationContext = null) => throw new NotImplementedException();
    public override Result UpdateMany(IEnumerable<TestEntity> updatedItems, IMap? validationContext = null) => throw new NotImplementedException();
    public override Result UpdateMany(Expression<Func<TestEntity, bool>> predicate, IEnumerable<TestEntity> updatedItems, IMap? validationContext = null) => throw new NotImplementedException();
}
