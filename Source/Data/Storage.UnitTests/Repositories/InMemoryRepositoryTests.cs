namespace DotNetToolbox.Data.Repositories;

public class InMemoryRepositoryTests {
    public record TestEntity(string Name);
    private readonly InMemoryRepository<TestEntity> _filledSet = [new("A"), new("B"), new("C")];

    private sealed class TestDataForRepositories : TheoryData<InMemoryRepository<TestEntity>, bool, int, TestEntity?, TestEntity[]> {
        public TestDataForRepositories() {
            Add([], false, 0, null, []);
            Add([new("A"), new("B"), new("C")], true, 3, new("A"), [new("A"), new("B"), new("C")]);
        }
    }
    [Theory]
    [ClassData(typeof(TestDataForRepositories))]
    public void Repository_ReturnsCorrectPropertyValues(InMemoryRepository<TestEntity> subject, bool haveAny, int count, TestEntity? first, TestEntity[] array) {
        subject.Any().Should().Be(haveAny);
        subject.Count().Should().Be(count);
        subject.FirstOrDefault().Should().Be(first);
        subject.ToArray().Should().BeEquivalentTo(array);
    }

    [Fact]
    public void Add_ReturnsNewCount() {
        _filledSet.Add(new("D"));
        _filledSet.Count().Should().Be(4);
    }

    [Fact]
    public void Update_ReturnsChangesItem() {
        _filledSet.Update(s => s.Name == "B", new("Z"));
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
