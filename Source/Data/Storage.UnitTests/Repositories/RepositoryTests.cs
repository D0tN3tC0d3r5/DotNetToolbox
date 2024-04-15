namespace DotNetToolbox.Data.Repositories;

public class RepositoryTests {
    public class TestEntity {
        public string Name { get; set; } = default!;
    };

    private sealed class TestDataForRepositories : TheoryData<Repository<TestEntity>, bool, int, TestEntity?, TestEntity[]> {
        public TestDataForRepositories() {
            Repository<TestEntity> emptySet = [];
            var one = new TestEntity();
            var two = new TestEntity();
            var three = new TestEntity();
            Repository<TestEntity> filledSet = [one, two, three];

            Add(emptySet, false, 0, null, []);
            Add(filledSet, true, 3, one, [one, two, three]);
        }
    }
    [Theory]
    [ClassData(typeof(TestDataForRepositories))]
    public void Repository_ReturnsCorrectPropertyValues(Repository<TestEntity> subject, bool haveAny, int count, TestEntity? first, TestEntity[] array) {
        subject.HaveAny().Should().Be(haveAny);
        subject.Count().Should().Be(count);
        subject.GetFirst().Should().Be(first);
        subject.ToArray().Should().BeEquivalentTo(array);
    }

    [Fact]
    public void Add_ReturnsNewCount() {
        var one = new TestEntity();
        var two = new TestEntity();
        var three = new TestEntity();
        var filledSet = new Repository<TestEntity> { one, two, three };
        var four = new TestEntity();

        filledSet.Add(four);
        filledSet.Count().Should().Be(4);
    }
}
