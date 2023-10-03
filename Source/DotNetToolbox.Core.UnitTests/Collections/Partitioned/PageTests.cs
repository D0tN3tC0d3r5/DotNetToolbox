namespace System.Collections.Partitioned;

public class PageTests {
    [Fact]
    public void Constructor_WithParams_Passes() {
        var subject = new Page<int>(totalCount: 12,
                                    items: new[] { 1, 2, 3, 4, 5, },
                                    offset: 1,
                                    size: 5);

        subject.TotalCount.Should().Be(12);
        subject.Items.Should().BeEquivalentTo(new[] { 1, 2, 3, 4, 5 });
        subject.Offset.Should().Be(1);
        subject.Size.Should().Be(5);
    }

    [Fact]
    public void ObjectInitializer_Passes() {
        var subject = new Page<int>() {
            Items = new[] { 1, 2, 3, 4, },
            Offset = 5,
            TotalCount = 100,
        };

        subject.Items.Should().BeEquivalentTo(new[] { 1, 2, 3, 4 });
        subject.Offset.Should().Be(5);
        subject.TotalCount.Should().Be(100);
        subject.Size.Should().Be(DefaultBlockSize);
    }

    [Fact]
    public void DefaultConstructor_Passes() {
        var subject = new Page<int>();

        subject.Items.Should().BeEmpty();
        subject.Offset.Should().Be(0);
        subject.TotalCount.Should().Be(0);
        subject.Size.Should().Be(DefaultBlockSize);
    }
}
