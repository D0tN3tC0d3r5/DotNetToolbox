namespace System.Collections.Partitioned;

public class PageTests {
    [Fact]
    public void Constructor_WithParams_Passes() {
        var subject = new Page<int>(totalCount: 12,
                                    items: [1, 2, 3, 4, 5,],
                                    offset: 1,
                                    size: 5);

        _ = subject.TotalCount.Should().Be(12);
        _ = subject.Items.Should().BeEquivalentTo([1, 2, 3, 4, 5,]);
        _ = subject.Offset.Should().Be(1);
        _ = subject.Size.Should().Be(5);
    }

    [Fact]
    public void ObjectInitializer_Passes() {
        var subject = new Page<int>() {
            Items = [1, 2, 3, 4,],
            Offset = 5,
            TotalCount = 100,
        };

        _ = subject.Items.Should().BeEquivalentTo([1, 2, 3, 4,]);
        _ = subject.Offset.Should().Be(5);
        _ = subject.TotalCount.Should().Be(100);
        _ = subject.Size.Should().Be(DefaultBlockSize);
    }

    [Fact]
    public void DefaultConstructor_Passes() {
        var subject = new Page<int>();

        _ = subject.Items.Should().BeEmpty();
        _ = subject.Offset.Should().Be(0);
        _ = subject.TotalCount.Should().Be(0);
        _ = subject.Size.Should().Be(DefaultBlockSize);
    }
}
