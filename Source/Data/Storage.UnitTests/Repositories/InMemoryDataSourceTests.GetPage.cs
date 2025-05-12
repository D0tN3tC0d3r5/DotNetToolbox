namespace DotNetToolbox.Data.Repositories;

public partial class InMemoryDataSourceTests {
    [Fact]
    public void GetPage_BaseStrategy_ShouldThrow() {
        var action = static () => _dummyDataSource.GetPage();
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public void GetPage_GetsAPage() {
        var firstItem = new TestEntity("0") { Id = 1 };
        var result = _readOnlyRepo.GetPage();
        result.Should().BeOfType<Page<TestEntity>>();
        result.Items.Count.Should().Be(20);
        result.Items[0].Should().BeEquivalentTo(firstItem);
    }

    [Fact]
    public void GetPage_WithIndex_GetsAPage() {
        var firstItem = new TestEntity("20") { Id = 21 };
        var result = _readOnlyRepo.GetPage(1);
        result.Should().BeOfType<Page<TestEntity>>();
        result.Items.Count.Should().Be(20);
        result.Items[0].Should().BeEquivalentTo(firstItem);
    }

    [Fact]
    public void GetLastPage_GetsAPage() {
        var firstItem = new TestEntity("80") { Id = 81 };
        var result = _readOnlyRepo.GetPage(4);
        result.Should().BeOfType<Page<TestEntity>>();
        result.Items.Count.Should().Be(10);
        result.Items[0].Should().BeEquivalentTo(firstItem);
    }
}
