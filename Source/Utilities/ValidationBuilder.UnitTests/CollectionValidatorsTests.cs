namespace DotNetToolbox.ValidationBuilder;

public sealed class CollectionValidatorsTests {
    public sealed record TestObject : IValidatable {
        public required ICollection<int> Numbers { get; init; } = Array.Empty<int>();
        public required ICollection<(string Name, int Age)> Names { get; init; } = Array.Empty<(string Name, int Age)>();
        public ICollection<string> Empty { get; } = Array.Empty<string>();

        public Result Validate(IMap? context = null) {
            var result = Success();
            result += Numbers.Is()
                .And().IsNotEmpty()
                .And().HasAtLeast(2)
                .And().HasAtMost(4)
                .And().Has(3)
                .And().Contains(5)
                .And().Each(item => item.Is().And().IsGreaterThan(0)).Result;
            result += Names.CheckIfEach(value => value.Name.IsRequired()).Result;
            result += Empty!.Is().And().IsEmpty().Result;
            return result;
        }
    }

    private sealed class TestData : TheoryData<TestObject, int> {
        public TestData() {
            Add(new() { Numbers = [1, 3, 5], Names = [("Name", 30)] }, 0);
            Add(new() { Numbers = [], Names = [("Name", 30), default!] }, 5);
            Add(new() { Numbers = [0, 5, 10, 13, 20], Names = [("Name", 30)] }, 3);
            Add(new() { Numbers = null!, Names = null! }, 2);
        }
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public void Validate_Validates(TestObject subject, int errorCount) {
        // Act
        var result = subject.Validate();

        // Assert
        result.Errors.Should().HaveCount(errorCount);
    }
}
