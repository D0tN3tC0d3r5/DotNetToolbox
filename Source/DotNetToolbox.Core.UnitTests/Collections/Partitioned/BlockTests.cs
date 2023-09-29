namespace System.Collections.Partitioned;

public class BlockTests
{
    private record BlockOfIntegers : Block<int>;

    [Fact]
    public void Constructor_WithParams_Passes()
    {
        var subject = new Block<int>(items: new[] { 1, 2, 3, 4, 5, },
                                     offset: "6",
                                     size: 5);

        subject.Items.Should().BeEquivalentTo(new[] { 1, 2, 3, 4, 5 });
        subject.Offset.Should().Be("6");
        subject.Size.Should().Be(5);
    }
}
