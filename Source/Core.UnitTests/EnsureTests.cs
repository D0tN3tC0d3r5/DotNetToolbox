namespace DotNetToolbox;

public class EnsureTests {
    [Fact]
    public void IsOfType_WhenArgumentIsOfWrongType_ThrowsArgumentNullException() {
        const int input = 12;
        var action = () => IsOfType<string>(input);
        action.Should().Throw<ArgumentException>().WithMessage("The value must be of type 'String'. Found: 'Int32'. (Parameter 'input')");
    }

    [Fact]
    public void IsOfType_WhenArgumentIsOfRightType_ReturnsInput() {
        const string input = "value";
        var result = IsOfType<string>(input);
        result.Should().BeSameAs(input);
    }

    [Fact]
    public void NotNull_WhenArgumentIsNull_ThrowsArgumentNullException() {
        const object? input = null;
        var action = () => IsNotNull(input);
        action.Should().Throw<ArgumentNullException>().WithMessage("The value cannot be null. (Parameter 'input')");
    }

    [Fact]
    public void NotNull_WhenArgumentIsNotNull_ReturnsInput() {
        var input = new object();
        var result = IsNotNull(input);
        result.Should().BeSameAs(input);
    }

    [Fact]
    public void DoesNotHaveValue_WhenArgumentIsNullableValueType_ThrowsArgumentNullException() {
        int? input = null;
        var action = () => IsNotNull(input);
        action.Should().Throw<ArgumentNullException>().WithMessage("The value cannot be null. (Parameter 'input')");
    }

    [Fact]
    public void DoesNotHaveValue_WhenArgumentIsNullableValueType_ReturnsInput() {
        int? input = 3;
        var result = IsNotNull(input);
        result.Should().Be(input);
    }

    [Fact]
    public void NotNullOrEmpty_WhenStringIsNull_ThrowsArgumentException() {
        const string? input = null;
        var action = () => IsNotNullOrEmpty(input);
        action.Should().Throw<ArgumentException>().WithMessage("The value cannot be null or empty. (Parameter 'input')");
    }

    [Fact]
    public void NotNullOrEmpty_WhenStringIsEmpty_ThrowsArgumentException() {
        var input = string.Empty;
        var action = () => IsNotNullOrEmpty(input);
        action.Should().Throw<ArgumentException>().WithMessage("The value cannot be null or empty. (Parameter 'input')");
    }

    [Fact]
    public void NotNullOrEmpty_WhenStringIsNotEmpty_ReturnsInput() {
        const string input = "Hello";
        var result = IsNotNullOrEmpty(input);
        result.Should().Be(input);
    }

    [Fact]
    public void NotNullOrWhiteSpace_WhenStringIsNull_ThrowsArgumentException() {
        const string? input = null;
        var action = () => IsNotNullOrWhiteSpace(input);
        action.Should().Throw<ArgumentException>().WithMessage("The value cannot be null or white space. (Parameter 'input')");
    }

    [Fact]
    public void NotNullOrWhiteSpace_WhenStringIsEmpty_ThrowsArgumentException() {
        var input = string.Empty;
        var action = () => IsNotNullOrWhiteSpace(input);
        action.Should().Throw<ArgumentException>().WithMessage("The value cannot be null or white space. (Parameter 'input')");
    }

    [Fact]
    public void NotNullOrWhiteSpace_WhenStringIsWhiteSpace_ThrowsArgumentException() {
        const string input = " ";
        var action = () => IsNotNullOrWhiteSpace(input);
        action.Should().Throw<ArgumentException>().WithMessage("The value cannot be null or white space. (Parameter 'input')");
    }

    [Fact]
    public void NotNullOrWhiteSpace_WhenStringIsNotEmpty_ReturnsInput() {
        const string input = "Hello";
        var result = IsNotNullOrWhiteSpace(input);
        result.Should().Be(input);
    }

    [Fact]
    public void DoesNotHaveNull_WhenDoesNotHaveNull_ThrowsArgumentException() {
        var input = new[] { default(int?) };
        var action = () => AllAreNotNull(input);
        action.Should().Throw<ValidationException>().WithMessage("input: The collection contains invalid elements.");
    }

    [Fact]
    public void DoesNotHaveNullOrEmpty_WhenDoesNotHaveEmpty_ThrowsArgumentException() {
        var input = new[] { string.Empty };
        var action = () => AllAreNotNullOrEmpty(input);
        action.Should().Throw<ValidationException>().WithMessage("input: The collection contains invalid elements.");
    }

    [Fact]
    public void DoesNotHaveNullOrEmpty_WhenValid_ThrowsArgumentException() {
        var input = new[] { "Hello" };
        var result = AllAreNotNullOrEmpty(input);
        result.Should().BeSameAs(input);
    }

    [Fact]
    public void NotNullOrDoesNotHaveNullOrWhiteSpace_WhenValid_ThrowsArgumentException() {
        var input = new[] { "Hello" };
        var result = AllAreNotNullOrWhiteSpace(input);
        result.Should().BeSameAs(input);
    }

    [Fact]
    public void NotNullOrDoesNotHaveNull_WhenIsNotEmpty_ReturnsInput() {
        var input = new[] { 1, 2, 3 };
        var result = AllAreNotNull(input);
        result.Should().BeSameAs(input);
    }

    [Fact]
    public void NotNullOrEmpty_WhenIsNull_ThrowsArgumentException() {
        const ICollection<int> input = default!;
        var result = IsNotEmpty(input);
        result.Should().BeNull();
    }

    [Fact]
    public void NotNullOrEmpty_WhenIsEmpty_ThrowsArgumentException() {
        var input = Array.Empty<int>();
        var action = () => IsNotNullOrEmpty(input);
        action.Should().Throw<ArgumentException>().WithMessage("The value cannot be null or empty. (Parameter 'input')");
    }

    [Fact]
    public void NoEmpty_WhenIsEmpty_ThrowsArgumentException() {
        var input = Array.Empty<int>();
        var action = () => IsNotEmpty(input);
        action.Should().Throw<ArgumentException>().WithMessage("The value cannot be empty. (Parameter 'input')");
    }

    [Fact]
    public void NotNullOrEmpty_WhenIsNotEmpty_ReturnsInput() {
        var input = new[] { 1, 2, 3 };
        var result = IsNotEmpty(input);
        result.Should().BeSameAs(input);
    }

    [Fact]
    public void DoesNotHaveNulls_WhenDoesNotHaveNull_ThrowsArgumentException() {
        var input = new[] { default(int?) };
        var action = () => AllAreNotNull(input);
        action.Should().Throw<ValidationException>().WithMessage("input: The collection contains invalid elements.");
    }

    [Fact]
    public void DoesNotHaveNulls_WhenValid_ReturnsSame() {
        var input = new[] { "hello" };
        var result = AllAreNotNull(input);
        result.Should().BeSameAs(input);
    }

    [Fact]
    public void DoesNotHaveNullOrEmptyStrings_WhenDoesNotHaveEmpty_ThrowsArgumentException() {
        var input = new[] { string.Empty };
        var action = () => AllAreNotNullOrEmpty(input);
        action.Should().Throw<ValidationException>().WithMessage("input: The collection contains invalid elements.");
    }

    [Fact]
    public void DoesNotHaveNullOrEmptyStrings_WhenValid_ReturnsSame() {
        var input = new[] { "Hello" };
        var result = AllAreNotNullOrEmpty(input);
        result.Should().BeSameAs(input);
    }

    [Fact]
    public void DoesNotHaveNullOrWhiteSpaceStrings_WhenDoesNotHaveWhitespace_ThrowsArgumentException() {
        var input = new[] { " " };
        var action = () => AllAreNotNullOrWhiteSpace(input);
        action.Should().Throw<ValidationException>().WithMessage("input: The collection contains invalid elements.");
    }

    private class ValidatableObject(bool isValid) : IValidatable {
        public Result Validate(IDictionary<string, object?>? context = null)
            => isValid ? Result.Success() : Result.Invalid("Is not valid.", "Source");
    }

    [Fact]
    public void IsValid_WhenInvalidValidatable_ThrowsValidationException() {
        var input = new ValidatableObject(isValid: false);
        var result = () => IsValid(input);
        result.Should().Throw<ValidationException>();
    }

    [Fact]
    public void IsValid_WhenValidValidatable_ReturnsSame() {
        var input = new ValidatableObject(isValid: true);
        var result = IsValid(input);
        result.Should().BeSameAs(input);
    }

    [Fact]
    public void IsValid_WhenValidSimpleObject_ReturnsSame() {
        var input = new object();
        var result = IsValid(input, _ => Result.Success());
        result.Should().BeSameAs(input);
    }

    [Fact]
    public void IsValid_WhenInvalidSimpleObject_ReturnsSame() {
        var input = new object();
        var result = () => IsValid(input, _ => Result.Invalid("Some error."));
        result.Should().Throw<ValidationException>();
    }

    [Fact]
    public void IsValid_WhenValidSimpleObject_AndPredicate_ReturnsSame() {
        var input = new object();
        var result = IsValid(input, _ => true);
        result.Should().BeSameAs(input);
    }

    [Fact]
    public void IsValid_WhenInvalidSimpleObject_AndPredicate_ReturnsSame() {
        var input = new object();
        var result = () => IsValid(input, _ => false);
        result.Should().Throw<ValidationException>();
    }
}
