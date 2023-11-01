namespace System;

public class EnsureTests {
    [Fact]
    public void IsOfType_WhenArgumentIsOfWrongType_ThrowsArgumentNullException() {
        const int input = 12;
        var action = () => IsOfType<string>(input);
        _ = action.Should().Throw<ArgumentException>().WithMessage("Value must be of type 'String'. Found: 'Int32'. (Parameter 'input')");
    }

    [Fact]
    public void IsOfType_WhenArgumentIsOfRightType_ReturnsInput() {
        const string input = "value";
        var result = IsOfType<string>(input);
        _ = result.Should().BeSameAs(input);
    }

    [Fact]
    public void NotNull_WhenArgumentIsNull_ThrowsArgumentNullException() {
        const object? input = null;
        var action = () => IsNotNull(input);
        _ = action.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'input')");
    }

    [Fact]
    public void NotNull_WhenArgumentIsNotNull_ReturnsInput() {
        var input = new object();
        var result = IsNotNull(input);
        _ = result.Should().BeSameAs(input);
    }

    [Fact]
    public void DoesNotHaveValue_WhenArgumentIsNullableValueType_ThrowsArgumentNullException() {
        int? input = null;
        var action = () => HasValue(input);
        _ = action.Should().Throw<ArgumentNullException>().WithMessage("Value cannot be null. (Parameter 'input')");
    }

    [Fact]
    public void DoesNotHaveValue_WhenArgumentIsNullableValueType_ReturnsInput() {
        int? input = 3;
        var result = HasValue(input);
        _ = result.Should().Be(input);
    }

    [Fact]
    public void NotNullOrEmpty_WhenStringIsNull_ThrowsArgumentException() {
        const string? input = null;
        var action = () => IsNotNullOrEmpty(input);
        _ = action.Should().Throw<ArgumentException>().WithMessage("Value cannot be null or empty. (Parameter 'input')");
    }

    [Fact]
    public void NotNullOrEmpty_WhenStringIsEmpty_ThrowsArgumentException() {
        var input = string.Empty;
        var action = () => IsNotNullOrEmpty(input);
        _ = action.Should().Throw<ArgumentException>().WithMessage("Value cannot be null or empty. (Parameter 'input')");
    }

    [Fact]
    public void NotNullOrEmpty_WhenStringIsNotEmpty_ReturnsInput() {
        const string input = "Hello";
        var result = IsNotNullOrEmpty(input);
        _ = result.Should().Be(input);
    }

    [Fact]
    public void NotNullOrWhiteSpace_WhenStringIsNull_ThrowsArgumentException() {
        const string? input = null;
        var action = () => IsNotNullOrWhiteSpace(input);
        _ = action.Should().Throw<ArgumentException>().WithMessage("Value cannot be null or white space. (Parameter 'input')");
    }

    [Fact]
    public void NotNullOrWhiteSpace_WhenStringIsEmpty_ThrowsArgumentException() {
        var input = string.Empty;
        var action = () => IsNotNullOrWhiteSpace(input);
        _ = action.Should().Throw<ArgumentException>().WithMessage("Value cannot be null or white space. (Parameter 'input')");
    }

    [Fact]
    public void NotNullOrWhiteSpace_WhenStringIsWhiteSpace_ThrowsArgumentException() {
        const string input = " ";
        var action = () => IsNotNullOrWhiteSpace(input);
        _ = action.Should().Throw<ArgumentException>().WithMessage("Value cannot be null or white space. (Parameter 'input')");
    }

    [Fact]
    public void NotNullOrWhiteSpace_WhenStringIsNotEmpty_ReturnsInput() {
        const string input = "Hello";
        var result = IsNotNullOrWhiteSpace(input);
        _ = result.Should().Be(input);
    }

    [Fact]
    public void DoesNotHaveNull_WhenDoesNotHaveNull_ThrowsArgumentException() {
        var input = new[] { default(int?), };
        var action = () => DoesNotHaveNulls(input);
        _ = action.Should().Throw<ArgumentException>().WithMessage("Value cannot contain null value(s). (Parameter 'input')");
    }

    [Fact]
    public void DoesNotHaveNullOrEmpty_WhenDoesNotHaveEmpty_ThrowsArgumentException() {
        var input = new[] { string.Empty, };
        var action = () => DoesNotHaveNullOrEmptyStrings(input);
        _ = action.Should().Throw<ArgumentException>().WithMessage("Value cannot contain null or empty string(s). (Parameter 'input')");
    }

    [Fact]
    public void DoesNotHaveNullOrEmpty_WhenValid_ThrowsArgumentException() {
        var input = new[] { "Hello", };
        var result = DoesNotHaveNullOrEmptyStrings(input);
        _ = result.Should().BeSameAs(input);
    }

    [Fact]
    public void NotNullOrDoesNotHaveNullOrWhiteSpace_WhenValid_ThrowsArgumentException() {
        var input = new[] { "Hello", };
        var result = DoesNotHaveNullOrWhiteSpaceStrings(input);
        _ = result.Should().BeSameAs(input);
    }

    [Fact]
    public void NotNullOrDoesNotHaveNull_WhenIsNotEmpty_ReturnsInput() {
        var input = new[] { 1, 2, 3, };
        var result = DoesNotHaveNulls(input);
        _ = result.Should().BeSameAs(input);
    }

    [Fact]
    public void NotNullOrEmpty_WhenIsNull_ThrowsArgumentException() {
        const ICollection<int> input = default!;
        var result = IsNotEmpty(input);
        _ = result.Should().BeNull();
    }

    [Fact]
    public void NotNullOrEmpty_WhenIsEmpty_ThrowsArgumentException() {
        var input = Array.Empty<int>();
        var action = () => IsNotNullOrEmpty(input);
        _ = action.Should().Throw<ArgumentException>().WithMessage("Value cannot be null or empty. (Parameter 'input')");
    }

    [Fact]
    public void NoEmpty_WhenIsEmpty_ThrowsArgumentException() {
        var input = Array.Empty<int>();
        var action = () => IsNotEmpty(input);
        _ = action.Should().Throw<ArgumentException>().WithMessage("Value cannot be empty. (Parameter 'input')");
    }

    [Fact]
    public void NotNullOrEmpty_WhenIsNotEmpty_ReturnsInput() {
        var input = new[] { 1, 2, 3, };
        var result = IsNotEmpty(input);
        _ = result.Should().BeSameAs(input);
    }

    [Fact]
    public void DoesNotHaveNulls_WhenDoesNotHaveNull_ThrowsArgumentException() {
        var input = new[] { default(int?), };
        var action = () => DoesNotHaveNulls(input);
        _ = action.Should().Throw<ArgumentException>().WithMessage("Value cannot contain null value(s). (Parameter 'input')");
    }

    [Fact]
    public void DoesNotHaveNulls_WhenValid_ReturnsSame() {
        var input = new[] { "hello", };
        var result = DoesNotHaveNulls(input);
        _ = result.Should().BeSameAs(input);
    }

    [Fact]
    public void DoesNotHaveNullOrEmptyStrings_WhenDoesNotHaveEmpty_ThrowsArgumentException() {
        var input = new[] { string.Empty, };
        var action = () => DoesNotHaveNullOrEmptyStrings(input);
        _ = action.Should().Throw<ArgumentException>().WithMessage("Value cannot contain null or empty string(s). (Parameter 'input')");
    }

    [Fact]
    public void DoesNotHaveNullOrEmptyStrings_WhenValid_ReturnsSame() {
        var input = new[] { "Hello", };
        var result = DoesNotHaveNullOrEmptyStrings(input);
        _ = result.Should().BeSameAs(input);
    }

    [Fact]
    public void DoesNotHaveNullOrWhiteSpaceStrings_WhenDoesNotHaveWhitespace_ThrowsArgumentException() {
        var input = new[] { " ", };
        var action = () => DoesNotHaveNullOrWhiteSpaceStrings(input);
        _ = action.Should().Throw<ArgumentException>().WithMessage("Value cannot contain null or white space string(s). (Parameter 'input')");
    }

    private class ValidatableObject(bool isValid)
        : IValidatable {
        public Result Validate(IDictionary<string, object?>? context = null)
            => isValid ? Success() : Invalid("Source", "Is not valid.");
    }

    [Fact]
    public void IsValid_WhenInvalidValidatable_ThrowsValidationException() {
        var input = new ValidatableObject(isValid: false);
        var result = () => IsValid(input);
        _ = result.Should().Throw<ValidationException>();
    }

    [Fact]
    public void IsValid_WhenValidValidatable_ReturnsSame() {
        var input = new ValidatableObject(isValid: true);
        var result = IsValid(input);
        _ = result.Should().BeSameAs(input);
    }

    [Fact]
    public void IsValid_WhenValidSimpleObject_ReturnsSame() {
        var input = new object();
        var result = IsValid(input, o => Success());
        _ = result.Should().BeSameAs(input);
    }

    [Fact]
    public void IsValid_WhenInvalidSimpleObject_ReturnsSame() {
        var input = new object();
        var result = () => IsValid(input, o => Invalid("Some error."));
        _ = result.Should().Throw<ValidationException>();
    }

    [Fact]
    public void IsValid_WhenValidSimpleObject_AndPredicate_ReturnsSame() {
        var input = new object();
        var result = IsValid(input, o => true);
        _ = result.Should().BeSameAs(input);
    }

    [Fact]
    public void IsValid_WhenInvalidSimpleObject_AndPredicate_ReturnsSame() {
        var input = new object();
        var result = () => IsValid(input, o => false);
        _ = result.Should().Throw<ValidationException>();
    }
}
