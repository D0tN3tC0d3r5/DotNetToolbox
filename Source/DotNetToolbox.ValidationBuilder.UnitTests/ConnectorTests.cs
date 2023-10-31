namespace DotNetToolbox.ValidationBuilder;

public class ConnectorTests {
    [Theory]
    [InlineData("value", 0)]
    [InlineData("error", 1)]
    [InlineData("val", 1)]
    [InlineData("err", 2)]
    public void And_AccumulatesErrors(string value, int expectedErrorCount) {
        // Arrange
        var validator = CreateValidatorFor(value);

        // Act
        var result = validator.LengthIs(5).And().Contains("val").Result;

        // Assert
        result.Errors.Should().HaveCount(expectedErrorCount);
    }

    [Theory]
    [InlineData("value", 0)]
    [InlineData("error", 0)]
    [InlineData("val", 0)]
    [InlineData("err", 2)]
    public void Or_AccumulatesErrors(string value, int expectedErrorCount) {
        // Arrange
        var validator = CreateValidatorFor(value);

        // Act
        var result = validator.LengthIs(5).Or().Contains("val").Result;

        // Assert
        result.Errors.Should().HaveCount(expectedErrorCount);
    }

    [Theory]
    [InlineData("value", 1)]
    [InlineData("error", 0)]
    [InlineData("val", 2)]
    [InlineData("err", 1)]
    public void AndNot_AccumulatesErrors(string value, int expectedErrorCount) {
        // Arrange
        var validator = CreateValidatorFor(value);

        // Act
        var result = validator.LengthIs(5).AndNot().Contains("val").Result;

        // Assert
        result.Errors.Should().HaveCount(expectedErrorCount);
    }

    [Theory]
    [InlineData("value", 0)]
    [InlineData("error", 0)]
    [InlineData("val", 2)]
    [InlineData("err", 0)]
    public void OrNot_AccumulatesErrors(string value, int expectedErrorCount) {
        // Arrange
        var validator = CreateValidatorFor(value);

        // Act
        var result = validator.LengthIs(5).OrNot().Contains("val").Result;

        // Assert
        result.Errors.Should().HaveCount(expectedErrorCount);
    }

    [Theory]
    [InlineData("value", 0)]
    [InlineData("error", 1)]
    [InlineData("val", 1)]
    [InlineData("err", 2)]
    public void AndClause_AccumulatesErrors(string value, int expectedErrorCount) {
        // Arrange
        var validator = CreateValidatorFor(value);

        // Act
        var result = validator.LengthIs(5).And(i => i.Contains("val")).Result;

        // Assert
        result.Errors.Should().HaveCount(expectedErrorCount);
    }

    [Theory]
    [InlineData("value", 0)]
    [InlineData("error", 0)]
    [InlineData("val", 0)]
    [InlineData("err", 2)]
    public void OrClause_AccumulatesErrors(string value, int expectedErrorCount) {
        // Arrange
        var validator = CreateValidatorFor(value);

        // Act
        var result = validator.LengthIs(5).Or(i => i.Contains("val")).Result;

        // Assert
        result.Errors.Should().HaveCount(expectedErrorCount);
    }

    [Theory]
    [InlineData("value", 1)]
    [InlineData("error", 0)]
    [InlineData("val", 2)]
    [InlineData("err", 1)]
    public void AndNotClause_AccumulatesErrors(string value, int expectedErrorCount) {
        // Arrange
        var validator = CreateValidatorFor(value);

        // Act
        var result = validator.LengthIs(5).AndNot(i => i.Contains("val")).Result;

        // Assert
        result.Errors.Should().HaveCount(expectedErrorCount);
    }

    [Theory]
    [InlineData("value", 0)]
    [InlineData("error", 0)]
    [InlineData("val", 2)]
    [InlineData("err", 0)]
    public void OrNotClause_AccumulatesErrors(string value, int expectedErrorCount) {
        // Arrange
        var validator = CreateValidatorFor(value);

        // Act
        var result = validator.LengthIs(5).OrNot(i => i.Contains("val")).Result;

        // Assert
        result.Errors.Should().HaveCount(expectedErrorCount);
    }

    private static StringValidator CreateValidatorFor(string value, ValidatorMode mode = ValidatorMode.And)
        => new(value, "Source", mode);
}