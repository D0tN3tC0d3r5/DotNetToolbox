namespace DotNetToolbox.Collections;

public class PaginationTests
{
    [Fact]
    public void Default_Passes()
    {
        var subject = Pagination.Default;

        subject.PageIndex.Should().Be(0);
        subject.PageSize.Should().Be(DefaultPageSize);
    }

    [Fact]
    public void Constructor_WithDefaults_SetsDefaultValues()
    {
        var subject = new Pagination();

        subject.PageIndex.Should().Be(0);
        subject.PageSize.Should().Be(DefaultPageSize);
        MinSize.Should().Be(1);
        MaxSize.Should().Be(1_000);
        MaxIndex.Should().Be(999_999_999);
        MaxCount.Should().Be(1_000_000_000);

        DefaultPageSize.Should().Be(20);
    }

    [Fact]
    public void Constructor_WithTotalCount_AdjustsPageIndex()
    {
        var subject = new Pagination(pageIndex: 10, totalCount: 100);

        subject.PageIndex.Should().Be(4U);
        subject.PageSize.Should().Be(DefaultPageSize);
    }

    [Fact]
    public void Constructor_WithZeroCount_AdjustsPageIndex()
    {
        var subject = new Pagination(pageIndex: 10, totalCount: 0);

        subject.PageIndex.Should().Be(0);
        subject.PageSize.Should().Be(DefaultPageSize);
    }

    [Fact]
    public void Constructor_WithValidPageSize_SetsPageSize()
    {
        const uint newSize = 37U;
        var subject = new Pagination(0, pageSize: newSize);

        subject.PageSize.Should().Be(newSize);
    }

    [Fact]
    public void Constructor_WithPageSizeGreaterThanMax_SetsPageSizeToMax()
    {
        var subject = new Pagination(0, pageSize: MaxSize + 1);

        subject.PageIndex.Should().Be(0);
        subject.PageSize.Should().Be(MaxSize);
    }

    [Fact]
    public void Constructor_WithPageIndexGreaterThanMax_SetsPageIndexToMax()
    {
        var subject = new Pagination(MaxIndex + 1, 1);

        subject.PageIndex.Should().Be(MaxIndex);
        subject.PageSize.Should().Be(1);
    }
}
