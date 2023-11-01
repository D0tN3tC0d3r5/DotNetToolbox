namespace DotNetToolbox.ValidationBuilder;

public class TypeValidatorsTests {
    public record TestObject : IValidatable {
        public Type? Type { get; init; }

        public Result Validate(IDictionary<string, object?>? context = null) {
            var result = Success();
            result += Type.Is()
                .And().IsEqualTo<string>().Result;
            return result;
        }
    }

    private class TestData : TheoryData<TestObject, int> {
        public TestData() {
            Add(new() { Type = typeof(string), }, 0);
            Add(new() { Type = null, }, 1);
            Add(new() { Type = typeof(int), }, 1);
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