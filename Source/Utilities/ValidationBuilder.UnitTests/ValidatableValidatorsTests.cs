namespace DotNetToolbox.ValidationBuilder;

public sealed class ValidatableValidatorsTests {
    public record ChildObject : IValidatable {
        public required string Name { get; init; }

        public Result Validate(IDictionary<string, object?>? context = null) {
            var result = Success();
            result += Name.IsRequired()
                .And().LengthIs(5).Result;
            return result;
        }
    }

    public record TestObject : IValidatable {
        public required ChildObject Child { get; init; }

        public Result Validate(IDictionary<string, object?>? context = null) {
            var result = Success();
            result += Child.IsRequired()
                .And().IsValid().Result;
            return result;
        }
    }

    private sealed class TestData : TheoryData<TestObject, bool, int> {
        public TestData() {
            Add(new() { Child = new() { Name = "Mario" } }, true, 0);
            Add(new() { Child = new() { Name = default! } }, false, 1);
            Add(new() { Child = default! }, false, 1);
        }
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public void Validate_Validates(TestObject subject, bool isSuccess, int errorCount) {
        // Act
        var result = subject.Validate();

        // Assert
        result.IsSuccess.Should().Be(isSuccess);
        result.Errors.Should().HaveCount(errorCount);
    }
}
