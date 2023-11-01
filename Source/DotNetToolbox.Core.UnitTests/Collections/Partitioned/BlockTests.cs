namespace System.Collections.Partitioned;

public class BlockTests {
    [Fact]
    public void Constructor_WithParams_Passes() {
        var subject = new Block<int>(items: new[] { 1, 2, 3, 4, 5 },
                                     offset: 6,
                                     size: 5);

        subject.Items.Should().BeEquivalentTo(new[] { 1, 2, 3, 4, 5 });
        subject.Offset.Should().Be(6);
        subject.Size.Should().Be(5);
    }

    [Fact]
    public void ObjectInitializer_Passes() {
        var subject = new Block<int>() {
            Items = new[] { 1, 2, 3, 4 },
            Offset = 5,
        };

        subject.Items.Should().BeEquivalentTo(new[] { 1, 2, 3, 4 });
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
