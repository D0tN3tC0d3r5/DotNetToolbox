namespace System.Collections.Partitioned;

public class BlockTests {
    [Fact]
    public void Constructor_WithParams_Passes() {
        var subject = new Block<int>(items: [1, 2, 3, 4, 5,],
                                     offset: 6,
                                     size: 5);

        _ = subject.Items.Should().BeEquivalentTo([1, 2, 3, 4, 5,]);
        _ = subject.Offset.Should().Be(6);
        _ = subject.Size.Should().Be(5);
    }

    [Fact]
    public void ObjectInitializer_Passes() {
        var subject = new Block<int>() {
            Items = [1, 2, 3, 4,],
            Offset = 5,
        };

        _ = subject.Items.Should().BeEquivalentTo([1, 2, 3, 4,]);
        _ = subject.Offset.Should().Be(5);
        _ = subject.Size.Should().Be(DefaultBlockSize);
    }

    [Fact]
    public void DefaultConstructor_Passes() {
        var subject = new Block<int>();

        _ = subject.Items.Should().BeEmpty();
        _ = subject.Offset.Should().Be(0);
        _ = subject.Size.Should().Be(DefaultBlockSize);
    }
}
