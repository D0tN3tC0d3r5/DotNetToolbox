namespace DotNetToolbox.Pagination;

public class BlockTests {
    [Fact]
    public void Constructor_WithParams_Passes() {
        var subject = new Block<int>(items: [1, 2, 3, 4, 5],
                                     offset: 6,
                                     size: 5);

        subject.Items.Should().BeEquivalentTo([1, 2, 3, 4, 5]);
        subject.Offset.Should().Be(6);
        subject.Size.Should().Be(5);
    }

    [Fact]
    public void ObjectInitializer_Passes() {
        var subject = new Block<int>() {
            Items = [1, 2, 3, 4],
            Offset = 5,
        };

        subject.Items.Should().BeEquivalentTo([1, 2, 3, 4]);
        subject.Offset.Should().Be(5);
        subject.Size.Should().Be(DefaultBlockSize);
    }

    [Fact]
    public void DefaultConstructor_Passes() {
        var subject = new Block<int>();

        subject.Items.Should().BeEmpty();
        subject.Offset.Should().Be(0);
        subject.Size.Should().Be(DefaultBlockSize);
    }
}
