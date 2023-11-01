namespace DotNetToolbox.ValidationBuilder;

public class ValidatorsTests {
    public class TestObject(long subject, string source, ValidatorMode mode = ValidatorMode.And) : Validator<long>(subject, source, mode);

    [Fact]
    public void Constructor_CreatesBuilder() {
        // Act
        var result = new TestObject(100, "SomeSubject");

        // Assert
        _ = result.Mode.Should().Be(ValidatorMode.And);
        _ = result.Result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Constructor_WithModeAndPreviousResult_CreatesBuilder() {
        // Act
        var result = new TestObject(100, "SomeSubject", ValidatorMode.Or);

        // Assert
        _ = result.Mode.Should().Be(ValidatorMode.Or);
        _ = result.Result.IsSuccess.Should().BeTrue();
    }
}