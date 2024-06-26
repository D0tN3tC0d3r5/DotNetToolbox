﻿namespace DotNetToolbox.Pagination;

public class PageTests {
    [Fact]
    public void Constructor_WithParams_Passes() {
        var subject = new Page<int>(items: [1, 2, 3, 4, 5],
                                    index: 1,
                                    size: 5,
                                    totalCount: 12);

        subject.TotalCount.Should().Be(12);
        subject.Items.Should().BeEquivalentTo([1, 2, 3, 4, 5]);
        subject.Index.Should().Be(1);
        subject.Size.Should().Be(5);
    }

    [Fact]
    public void ObjectInitializer_Passes() {
        var subject = new Page<int>() {
            Items = [1, 2, 3, 4],
            Index = 5,
            TotalCount = 100,
        };

        subject.Items.Should().BeEquivalentTo([1, 2, 3, 4]);
        subject.Index.Should().Be(5);
        subject.TotalCount.Should().Be(100);
        subject.Size.Should().Be(DefaultBlockSize);
    }

    [Fact]
    public void DefaultConstructor_Passes() {
        var subject = new Page<int>();

        subject.Items.Should().BeEmpty();
        subject.Index.Should().Be(0);
        subject.TotalCount.Should().Be(0);
        subject.Size.Should().Be(DefaultBlockSize);
    }
}
