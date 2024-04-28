namespace DotNetToolbox.Data.Repositories;

public partial class RepositoryTests {
    private abstract record BaseTestEntity(string Name);

    private sealed record TestEntity(string Name) : BaseTestEntity(Name);
    private sealed class TestRepository : Repository<TestEntity>;

    private static readonly Repository<TestEntity> _repo = [new("A"), new("BB"), new("CCC")];
    private readonly TestRepository _updatableRepo = [new("A"), new("BB"), new("CCC")];

    private sealed class DummyRepositoryStrategy : RepositoryStrategy<TestEntity>;
    private sealed class DummyRepository()
        : Repository<DummyRepositoryStrategy, TestEntity>([], []);
    private static readonly DummyRepository _dummyRepository = [];
}
