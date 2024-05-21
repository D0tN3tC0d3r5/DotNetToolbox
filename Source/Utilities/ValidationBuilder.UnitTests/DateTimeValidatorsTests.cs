namespace DotNetToolbox.ValidationBuilder;

public sealed class DateTimeValidatorsTests {
    private static readonly DateTime _baseDate = DateTime.Parse("2023-04-01 12:34:56.789");

    public sealed record TestObject : IValidatable {
        public DateTime NotNull { get; init; }
        public DateTime? Nullable { get; init; }
        public DateTime? Required { get; init; }

        public Result Validate(IDictionary<string, object?>? context = null) {
            var result = Success();
            result += NotNull.Is().And().IsAfter(_baseDate).And().IsBefore(_baseDate.AddDays(1)).Result;
            result += Nullable.IsOptional().And().IsAfter(_baseDate).And().IsBefore(_baseDate.AddDays(1)).Result;
            result += Required.IsRequired().And().StartsOn(_baseDate).And().EndsOn(_baseDate.AddDays(1)).Result;
            return result;
        }
    }

    private sealed class TestData : TheoryData<TestObject, bool, int> {
        public TestData() {
            Add(new() { NotNull = _baseDate.AddSeconds(1), Nullable = _baseDate.AddSeconds(1), Required = _baseDate.AddSeconds(1) }, true, 0);
            Add(new() { NotNull = _baseDate, Nullable = null, Required = null }, false, 2);
            Add(new() { NotNull = _baseDate.AddDays(-1), Nullable = _baseDate, Required = _baseDate.AddDays(-1) }, false, 3);
            Add(new() { NotNull = _baseDate.AddDays(3), Nullable = _baseDate, Required = _baseDate.AddDays(3) }, false, 3);
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
