namespace DotNetToolbox.ValidationBuilder;

public sealed class DecimalValidatorsTests {
    public sealed record TestObject : IValidatable {
        public decimal NonNull { get; init; }
        public decimal? Nullable { get; init; }
        public decimal? Required { get; init; }

        public Result Validate(IDictionary<string, object?>? context = null) {
            var result = Success();
            result += NonNull.Is().Result;
            result += Nullable.IsOptional().And().IsGreaterThan(10).And().IsLessThan(20).And().IsEqualTo(15).Result;
            result += Required.IsRequired().And().MinimumIs(10).And().MaximumIs(20).Result;
            return result;
        }
    }

    private sealed class TestData : TheoryData<TestObject, bool, int> {
        public TestData() {
            Add(new() { Nullable = 15, Required = 15 }, true, 0);
            Add(new() { Nullable = null, Required = null }, false, 1);
            Add(new() { Nullable = 5, Required = 5 }, false, 3);
            Add(new() { Nullable = 25, Required = 25 }, false, 3);
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
