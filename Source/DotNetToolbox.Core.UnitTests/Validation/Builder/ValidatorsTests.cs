namespace System.Validation.Builder;

public class ValidatorsTests {
    public class TestObject : Validator<long> {
        public TestObject(long subject, string source, ValidatorMode mode = ValidatorMode.And)
            : base(subject, source, mode) {
        }
    }

    [Fact]
    public void Constructor_CreatesBuilder() {
        // Act
        var result = new TestObject(100, "SomeSubject");

        // Assert
        result.Mode.Should().Be(ValidatorMode.And);
        result.Result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Constructor_WithModeAndPreviousResult_CreatesBuilder() {
        // Act
        var result = new TestObject(100, "SomeSubject", ValidatorMode.Or);

        // Assert
        result.Mode.Should().Be(ValidatorMode.Or);
        result.Result.IsSuccess.Should().BeTrue();
    }
}