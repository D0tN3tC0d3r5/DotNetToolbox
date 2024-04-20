namespace DotNetToolbox.Data.Repositories;

public class InMemoryRepositoryTests {
    private readonly InMemoryRepository<int> _filledSet = [1, 2, 3];

    private sealed class TestDataForRepositories : TheoryData<InMemoryRepository<int>, bool, int, int?, int[]> {
        public TestDataForRepositories() {
            Add([], false, 0, 0, []);
            Add([1, 2, 3], true, 3, 1, [1, 2, 3]);
        }
    }
    [Theory]
    [ClassData(typeof(TestDataForRepositories))]
    public void Repository_ReturnsCorrectPropertyValues(InMemoryRepository<int> subject, bool haveAny, int count, int? first, int[] array) {
        subject.Any().Should().Be(haveAny);
        subject.Count().Should().Be(count);
        subject.FirstOrDefault().Should().Be(first);
        subject.ToArray().Should().BeEquivalentTo(array);
    }

    [Fact]
    public void Add_ReturnsNewCount() {
        _filledSet.Add(4);
        _filledSet.Count().Should().Be(4);
    }

    [Fact]
    public void Update_ReturnsChangesItem() {
        _filledSet.Update(s => s == 2, 99);
        _filledSet.Count().Should().Be(3);
        _filledSet.FirstOrDefault(s => s == 2).Should().Be(0);
        _filledSet.FirstOrDefault(s => s == 99).Should().Be(99);
    }

    [Fact]
    public void Remove_ReturnsNewCount() {
        _filledSet.Remove(s => s == 2);
        _filledSet.Count().Should().Be(2);
    }
}
