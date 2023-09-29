namespace System.Collections.Partitioned;

public class PageTests
{
    private record PageOfIntegers : Page<int>;

    [Fact]
    public void Constructor_WithParams_Passes()
    {
        var subject = new Page<int>(totalCount: 12,
                                    items: new[] { 1, 2, 3, 4, 5, },
                                    offset: 1,
                                    size: 5);

        subject.TotalCount.Should().Be(12);
        subject.Items.Should().BeEquivalentTo(new[] { 1, 2, 3, 4, 5 });
        subject.Offset.Should().Be(1);
        subject.Size.Should().Be(5);
    }
}
