namespace DotNetToolbox.Data.Repositories;

public partial class RepositoryTests {
    [Fact]
    public void GetAsPaged_IfNotPaged_ReturnsNull() {
        var result = _dummyRepository.AsPaged();
        result.Should().BeNull();
    }

    [Fact]
    public void GetPage_BaseStrategy_ShouldThrow() {
        var action = () => _dummyPagedRepository.AsPaged()!.GetPage();
        action.Should().Throw<NotImplementedException>();
    }

    [Fact]
    public async Task GetPageAsync_BaseStrategy_ShouldThrow() {
        var action = async () => await _dummyPagedRepository.AsPaged()!.GetPageAsync();
        await action.Should().ThrowAsync<NotImplementedException>();
    }

    [Fact]
    public void GetPage_GetsAPage() {
        var result = _pagedRepo.AsPaged()!.GetPage();
        result.Should().BeOfType<Page<TestEntity>>();
        result.Items.Count().Should().Be(20);
    }

    [Fact]
    public async Task GetPageAsync_GetAPage() {
        var firstItem = new TestEntity("0");
        var result = await _pagedRepo.AsPaged()!.GetPageAsync();
        result.Should().BeOfType<Page<TestEntity>>();
        result.Items.Count().Should().Be(20);
        result.Items[0].Should().BeEquivalentTo(firstItem);
    }

    [Fact]
    public void GetPage_WithIndex_GetsAPage() {
        var firstItem = new TestEntity("20");
        var result = _pagedRepo.AsPaged()!.GetPage(1);
        result.Should().BeOfType<Page<TestEntity>>();
        result.Items.Count().Should().Be(20);
        result.Items[0].Should().BeEquivalentTo(firstItem);
    }

    [Fact]
    public async Task GetPageAsync_WithIndex_GetAPage() {
        var firstItem = new TestEntity("20");
        var result = await _pagedRepo.AsPaged()!.GetPageAsync(1);
        result.Should().BeOfType<Page<TestEntity>>();
        result.Items.Count().Should().Be(20);
        result.Items[0].Should().BeEquivalentTo(firstItem);
    }

    [Fact]
    public void GetLastPage_GetsAPage() {
        var firstItem = new TestEntity("80");
        var result = _pagedRepo.AsPaged()!.GetPage(4);
        result.Should().BeOfType<Page<TestEntity>>();
        result.Items.Count().Should().Be(10);
        result.Items[0].Should().BeEquivalentTo(firstItem);
    }

    [Fact]
    public async Task GetLastPageAsync_GetAPage() {
        var firstItem = new TestEntity("80");
        var result = await _pagedRepo.AsPaged()!.GetPageAsync(4);
        result.Should().BeOfType<Page<TestEntity>>();
        result.Items.Count().Should().Be(10);
        result.Items[0].Should().BeEquivalentTo(firstItem);
    }
}
