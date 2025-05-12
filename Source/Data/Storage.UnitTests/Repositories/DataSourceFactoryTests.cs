namespace DotNetToolbox.Data.Repositories;

public class DataSourceFactoryTests {
    private readonly DataSourceFactory _factory = new();

    [Fact]
    public void CreateRepository_ReturnsRepository() {
        var keyGenerator = Substitute.For<IKeyGenerator<uint>>();
        var result = _factory.Create<InMemoryDataSource<TestEntity>, InMemoryStorage<TestEntity>, TestEntity>("test", keyGenerator);

        result.Should().BeOfType<InMemoryDataSource<TestEntity>>();
    }

    [Fact]
    public void CreateRepository_WithData_ReturnsSeededRepository() {
        var keyGenerator = Substitute.For<IKeyGenerator<uint>>();
        var seed = new TestEntity[] { new(1, "One"), new(2, "Two"), new(3, "Three") };
        var result = _factory.Create<InMemoryDataSource<TestEntity>, InMemoryStorage<TestEntity>, TestEntity>("test", keyGenerator, seed);

        result.Should().BeOfType<InMemoryDataSource<TestEntity>>();
        result.Count().Should().Be(3);
    }

    [Fact]
    public void CreateRepository_FromStorage_ReturnsRepository() {
        var keyGenerator = Substitute.For<IKeyGenerator<uint>>();
        var storage = new InMemoryStorage<TestEntity>("test", keyGenerator);
        var result = _factory.Create<InMemoryDataSource<TestEntity>, InMemoryStorage<TestEntity>, TestEntity>(storage);

        result.Should().BeOfType<InMemoryDataSource<TestEntity>>();
    }
}
