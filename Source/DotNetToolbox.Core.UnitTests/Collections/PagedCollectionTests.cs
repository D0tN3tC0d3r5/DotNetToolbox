namespace DotNetToolbox.Collections;

public class PagedCollectionTests
{
    private record PagedIntegers : PagedCollection<PagedIntegers, int>;

    [Fact]
    public void PagedIntegers_Empty_Passes()
    {
        var subject = PagedIntegers.Empty;

        subject.PageSize.Should().Be(DefaultPageSize);
        subject.PageIndex.Should().Be(0);
        subject.TotalCount.Should().Be(0);
        subject.Items.ToArray().Should().BeEmpty();
    }

    [Fact]
    public void PagedCollection_Empty_Passes()
    {
        var subject = PagedCollection<int>.Empty;

        subject.TotalCount.Should().Be(0);
        subject.Items.ToArray().Should().BeEmpty();
        subject.PageIndex.Should().Be(0);
        subject.PageSize.Should().Be(DefaultPageSize);
    }

    [Fact]
    public void PagedCollection_WithParams_Passes()
    {
        var subject = new PagedCollection<int>
        {
            TotalCount = 12,
            Items = new[] { 1, 2, 3, 4, 5 },
            PageIndex = 1,
            PageSize = 5,
        };

        subject.TotalCount.Should().Be(12);
        subject.Items.Should().BeEquivalentTo(new[] { 1, 2, 3, 4, 5 });
        subject.PageIndex.Should().Be(1);
        subject.PageSize.Should().Be(5);
    }

    [Fact]
    public void PagedCollection_MapTo_ProjectsCollectionToADifferentItemType()
    {
        var list = new PagedCollection<int>
        {
            TotalCount = 12,
            Items = new[] { 1, 2, 3, 4, 5 },
            PageIndex = 0,
            PageSize = 5,
        };

        var subject = list.Map(i => i.ToString());

        subject.TotalCount.Should().Be(list.TotalCount);
        subject.Items.Should().BeEquivalentTo("1", "2", "3", "4", "5");
        subject.PageIndex.Should().Be(list.PageIndex);
        subject.PageSize.Should().Be(list.PageSize);
    }
}
