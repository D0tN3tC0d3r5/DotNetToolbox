namespace DotNetToolbox.Data.Repositories;

public class RepositoryTests {
    public class TestEntity(string name) {
        public string Name { get; } = name;
    }

    private readonly Repository<TestEntity> _filledSet = [new("A"), new("B"), new("C")];

    private sealed class TestDataForRepositories : TheoryData<Repository<TestEntity>, bool, int, TestEntity?, TestEntity[]> {
        public TestDataForRepositories() {
            var one = new TestEntity("One");
            var two = new TestEntity("Two");
            var three = new TestEntity("Three");
            Add([], false, 0, null, []);
            Add([one, two, three], true, 3, one, [one, two, three]);
        }
    }
    [Theory]
    [ClassData(typeof(TestDataForRepositories))]
    public void Repository_ReturnsCorrectPropertyValues(Repository<TestEntity> subject, bool haveAny, int count, TestEntity? first, TestEntity[] array) {
        subject.Any().Should().Be(haveAny);
        subject.Count().Should().Be(count);
        subject.FirstOrDefault().Should().Be(first);
        subject.ToArray().Should().BeEquivalentTo(array);
    }

    [Fact]
    public void Add_ReturnsNewCount() {
        var four = new TestEntity("D");
        _filledSet.Add(four);
        _filledSet.Count().Should().Be(4);
    }

    [Fact]
    public void Update_ReturnsChangesItem() {
        var other = new TestEntity("Z");
        _filledSet.Update(s => s.Name == "B", other);
        _filledSet.Count().Should().Be(3);
        _filledSet.FirstOrDefault(s => s.Name == "B").Should().BeNull();
        _filledSet.FirstOrDefault(s => s.Name == "Z").Should().NotBeNull();
    }

    [Fact]
    public void Remove_ReturnsNewCount() {
        _filledSet.Remove(s => s.Name == "B");
        _filledSet.Count().Should().Be(2);
    }
}
