using static DotNetToolbox.Utilities.Ensure;

namespace DotNetToolbox.Utilities;

public class EnsureTests
{
    [Fact]
    public void IsOfType_WhenArgumentIsOfWrongType_ThrowsArgumentNullException()
    {
        const int input = 12;
        var action = () => IsOfType<string>(input);
        action.Should().Throw<ArgumentException>().WithMessage("'input' must be of type 'String'. Found: 'Int32'. (Parameter 'input')");
    }

    [Fact]
    public void IsOfType_WhenArgumentIsOfRightType_ReturnsInput()
    {
        const string input = "value";
        var result = IsOfType<string>(input);
        result.Should().BeSameAs(input);
    }

    [Fact]
    public void NotNull_WhenArgumentIsNull_ThrowsArgumentNullException()
    {
        const object? input = null;
        var action = () => IsNotNull(input);
        action.Should().Throw<ArgumentNullException>().WithMessage("'input' cannot be null. (Parameter 'input')");
    }

    [Fact]
    public void NotNull_WhenArgumentIsNotNull_ReturnsInput()
    {
        var input = new object();
        var result = IsNotNull(input);
        result.Should().BeSameAs(input);
    }

    [Fact]
    public void HasValue_WhenArgumentIsNullableValueType_ThrowsArgumentNullException()
    {
        int? input = null;
        var action = () => HasValue(input);
        action.Should().Throw<ArgumentNullException>().WithMessage("'input' cannot be null. (Parameter 'input')");
    }

    [Fact]
    public void HasValue_WhenArgumentIsNullableValueType_ReturnsInput()
    {
        int? input = 3;
        var result = HasValue(input);
        result.Should().Be(input);
    }

    [Fact]
    public void NotNullOrEmpty_WhenStringIsNull_ThrowsArgumentException()
    {
        const string? input = null;
        var action = () => IsNotNullOrEmpty(input);
        action.Should().Throw<ArgumentException>().WithMessage("'input' cannot be null or empty. (Parameter 'input')");
    }

    [Fact]
    public void NotNullOrEmpty_WhenStringIsEmpty_ThrowsArgumentException()
    {
        var input = string.Empty;
        var action = () => IsNotNullOrEmpty(input);
        action.Should().Throw<ArgumentException>().WithMessage("'input' cannot be null or empty. (Parameter 'input')");
    }

    [Fact]
    public void NotNullOrEmpty_WhenStringIsNotEmpty_ReturnsInput()
    {
        const string input = "Hello";
        var result = IsNotNullOrEmpty(input);
        result.Should().Be(input);
    }

    [Fact]
    public void NotNullOrWhiteSpace_WhenStringIsNull_ThrowsArgumentException()
    {
        const string? input = null;
        var action = () => IsNotNullOrWhiteSpace(input);
        action.Should().Throw<ArgumentException>().WithMessage("'input' cannot be null or whitespace. (Parameter 'input')");
    }

    [Fact]
    public void NotNullOrWhiteSpace_WhenStringIsEmpty_ThrowsArgumentException()
    {
        var input = string.Empty;
        var action = () => IsNotNullOrWhiteSpace(input);
        action.Should().Throw<ArgumentException>().WithMessage("'input' cannot be null or whitespace. (Parameter 'input')");
    }

    [Fact]
    public void NotNullOrWhiteSpace_WhenStringIsWhiteSpace_ThrowsArgumentException()
    {
        const string input = " ";
        var action = () => IsNotNullOrWhiteSpace(input);
        action.Should().Throw<ArgumentException>().WithMessage("'input' cannot be null or whitespace. (Parameter 'input')");
    }

    [Fact]
    public void NotNullOrWhiteSpace_WhenStringIsNotEmpty_ReturnsInput()
    {
        const string input = "Hello";
        var result = IsNotNullOrWhiteSpace(input);
        result.Should().Be(input);
    }

    [Fact]
    public void NotNullOrHasNull_WhenIsNull_ThrowsArgumentException()
    {
        const ICollection<int> input = default!;
        var action = () => IsNotNullAndDoesNotContainNull(input);
        action.Should().Throw<ArgumentException>().WithMessage("'input' cannot be null. (Parameter 'input')");
    }

    [Fact]
    public void NotNullOrHasNull_WhenHasNull_ThrowsArgumentException()
    {
        var input = new[] { default(int?) };
        var action = () => IsNotNullAndDoesNotContainNull(input);
        action.Should().Throw<ArgumentException>().WithMessage("'input' cannot contain null item(s). (Parameter 'input')");
    }

    [Fact]
    public void NotNullOrHasNullOrEmpty_WhenHasEmpty_ThrowsArgumentException()
    {
        var input = new[] { string.Empty };
        var action = () => IsNotNullAndDoesNotContainNullOrEmpty(input);
        action.Should().Throw<ArgumentException>().WithMessage("'input' cannot contain null or empty string(s). (Parameter 'input')");
    }

    [Fact]
    public void NotNullOrHasNullOrEmpty_WhenValid_ThrowsArgumentException()
    {
        var input = new[] { "Hello" };
        var result = IsNotNullAndDoesNotContainNullOrEmpty(input);
        result.Should().BeSameAs(input);
    }

    [Fact]
    public void NotNullOrHasNullOrWhiteSpace_WhenHasWhitespace_ThrowsArgumentException()
    {
        var input = new[] { " " };
        var action = () => IsNotNullAndDoesNotContainNullOrWhiteSpace(input);
        action.Should().Throw<ArgumentException>().WithMessage("'input' cannot contain null or whitespace string(s). (Parameter 'input')");
    }

    [Fact]
    public void NotNullOrHasNullOrWhiteSpace_WhenValid_ThrowsArgumentException()
    {
        var input = new[] { "Hello" };
        var result = IsNotNullAndDoesNotContainNullOrWhiteSpace(input);
        result.Should().BeSameAs(input);
    }

    [Fact]
    public void NotNullOrHasNull_WhenIsNotEmpty_ReturnsInput()
    {
        var input = new[] { 1, 2, 3 };
        var result = IsNotNullAndDoesNotContainNull(input);
        result.Should().BeSameAs(input);
    }

    [Fact]
    public void NotNullOrEmpty_WhenIsNull_ThrowsArgumentException()
    {
        const ICollection<int> input = default!;
        var action = () => IsNotNullOrEmpty(input);
        action.Should().Throw<ArgumentException>().WithMessage("'input' cannot be null or empty. (Parameter 'input')");
    }

    [Fact]
    public void NotNullOrEmpty_WhenIsEmpty_ThrowsArgumentException()
    {
        var input = Array.Empty<int>();
        var action = () => IsNotNullOrEmpty(input);
        action.Should().Throw<ArgumentException>().WithMessage("'input' cannot be null or empty. (Parameter 'input')");
    }

    [Fact]
    public void NotNullOrEmpty_WhenIsNotEmpty_ReturnsInput()
    {
        var input = new[] { 1, 2, 3 };
        var result = IsNotNullOrEmpty(input);
        result.Should().BeSameAs(input);
    }

    [Fact]
    public void NotNullOrEmptyOrHasNull_WhenHasNull_ThrowsArgumentException()
    {
        var input = new[] { default(int?) };
        var action = () => IsNotNullOrEmptyAndDoesNotContainNull(input);
        action.Should().Throw<ArgumentException>().WithMessage("'input' cannot contain null item(s). (Parameter 'input')");
    }

    [Fact]
    public void NotNullOrEmptyOrHasNull_WhenValid_ReturnsSame()
    {
        var input = new[] { "hello" };
        var result = IsNotNullOrEmptyAndDoesNotContainNull(input);
        result.Should().BeSameAs(input);
    }

    [Fact]
    public void NotNullOrEmptyOrHasNullOrEmpty_WhenHasEmpty_ThrowsArgumentException()
    {
        var input = new[] { string.Empty };
        var action = () => IsNotNullOrEmptyAndDoesNotContainNullOrEmpty(input);
        action.Should().Throw<ArgumentException>().WithMessage("'input' cannot contain null or empty string(s). (Parameter 'input')");
    }

    [Fact]
    public void NotNullOrEmptyOrHasNullOrEmpty_WhenValid_ReturnsSame()
    {
        var input = new[] { "Hello" };
        var result = IsNotNullOrEmptyAndDoesNotContainNullOrEmpty(input);
        result.Should().BeSameAs(input);
    }

    [Fact]
    public void NotNullOrEmptyOrHasNullOrWhiteSpace_WhenHasWhitespace_ThrowsArgumentException()
    {
        var input = new[] { " " };
        var action = () => IsNotNullOrEmptyAndDoesNotContainNullOrWhiteSpace(input);
        action.Should().Throw<ArgumentException>().WithMessage("'input' cannot contain null or whitespace string(s). (Parameter 'input')");
    }

    [Fact]
    public void NotNullOrEmptyOrHasNullOrWhiteSpace_WhenEmpty_ThrowsArgumentException()
    {
        var input = Array.Empty<string>();
        var action = () => IsNotNullOrEmptyAndDoesNotContainNullOrWhiteSpace(input);
        action.Should().Throw<ArgumentException>().WithMessage("'input' cannot be null or empty. (Parameter 'input')");
    }

    [Fact]
    public void NotNullOrEmptyOrHasNullOrWhiteSpace_WhenHasNull_ThrowsArgumentException()
    {
        string[] input = { default! };
        var action = () => IsNotNullOrEmptyAndDoesNotContainNullOrWhiteSpace(input);
        action.Should().Throw<ArgumentException>().WithMessage("'input' cannot contain null or whitespace string(s). (Parameter 'input')");
    }

    [Fact]
    public void NotNullOrEmptyOrHasNullOrWhiteSpace_WhenHasEmpty_ThrowsArgumentException()
    {
        var input = new[] { string.Empty };
        var action = () => IsNotNullOrEmptyAndDoesNotContainNullOrWhiteSpace(input);
        action.Should().Throw<ArgumentException>().WithMessage("'input' cannot contain null or whitespace string(s). (Parameter 'input')");
    }

    [Fact]
    public void NotNullOrEmptyOrHasNullOrWhiteSpace_WhenValid_ReturnsSame()
    {
        var input = new[] { "Hello" };
        var result = IsNotNullOrEmptyAndDoesNotContainNullOrWhiteSpace(input);
        result.Should().BeSameAs(input);
    }

    [Fact]
    public void ArgumentExistsAndIsOfType_WhenEmpty_ThrowsArgumentException()
    {
        const string method = "MethodName";
        var arguments = Array.Empty<object?>();
        var action = () => ArgumentExistsAndIsOfType<string>(method, arguments, 0);
        action.Should().Throw<ArgumentException>().WithMessage("Invalid number of arguments for 'MethodName'. Missing argument 0. (Parameter 'arguments')");
    }

    [Fact]
    public void ArgumentExistsAndIsOfType_WhenWrongType_ThrowsArgumentException()
    {
        const string method = "MethodName";
        var arguments = new object?[] { 1, "2", 3 };
        var action = () => ArgumentExistsAndIsOfType<string>(method, arguments, 0);
        action.Should().Throw<ArgumentException>().WithMessage("Invalid type of arguments[0] of 'MethodName'. Expected: String. Found: Int32. (Parameter 'arguments[0]')");
    }

    [Fact]
    public void ArgumentExistsAndIsOfType_WhenValid_ReturnsItem()
    {
        const string method = "MethodName";
        var arguments = new object?[] { 1, "2", 3.0m };
        var value = ArgumentExistsAndIsOfType<int>(method, arguments, 0);
        value.Should().Be(1);
    }

    [Fact]
    public void ArgumentsAreAllOfType_WhenEmpty_ThrowsArgumentException()
    {
        const string method = "MethodName";
        var arguments = Array.Empty<object?>();
        var action = () => ArgumentsAreAllOfType<string>(method, arguments);
        action.Should().Throw<ArgumentException>().WithMessage("'arguments' cannot be null or empty. (Parameter 'arguments')");
    }

    [Fact]
    public void ArgumentsAreAllOfType_WhenWrongType_ThrowsArgumentException()
    {
        const string method = "MethodName";
        var arguments = new object?[] { 1, "2", 3 };
        var action = () => ArgumentsAreAllOfType<int>(method, arguments);
        action.Should().Throw<ArgumentException>().WithMessage("Invalid type of arguments[1] of 'MethodName'. Expected: Int32. Found: String. (Parameter 'arguments[1]')");
    }

    [Fact]
    public void ArgumentsAreAllOfType_WhenValid_ReturnsItem()
    {
        const string method = "MethodName";
        var arguments = new object?[] { 1, 2, 3 };
        var value = ArgumentsAreAllOfType<int>(method, arguments);
        value.Should().BeOfType<int[]>().Subject.Should().BeEquivalentTo(arguments);
    }

    [Fact]
    public void ArgumentExistsAndIsOfTypeOrDefault_WhenEmpty_ThrowsArgumentException()
    {
        const string method = "MethodName";
        var arguments = Array.Empty<object?>();
        var action = () => ArgumentExistsAndIsOfTypeOrDefault<string>(method, arguments, 0);
        action.Should().Throw<ArgumentException>().WithMessage("Invalid number of arguments for 'MethodName'. Missing argument 0. (Parameter 'arguments')");
    }

    [Fact]
    public void ArgumentExistsAndIsOfTypeOrDefault_WhenWrongType_ThrowsArgumentException()
    {
        const string method = "MethodName";
        var arguments = new object?[] { 1, "2", 3 };
        var action = () => ArgumentExistsAndIsOfTypeOrDefault<string>(method, arguments, 0);
        action.Should().Throw<ArgumentException>().WithMessage("Invalid type of arguments[0] of 'MethodName'. Expected: String. Found: Int32. (Parameter 'arguments[0]')");
    }

    [Fact]
    public void ArgumentExistsAndIsOfTypeOrDefault_WhenValid_ReturnsItem()
    {
        const string method = "MethodName";
        var arguments = new object?[] { 1, "2", 3.0m };
        var value = ArgumentExistsAndIsOfTypeOrDefault<int>(method, arguments, 0);
        value.Should().Be(1);
    }

    [Fact]
    public void ArgumentExistsAndIsOfTypeOrDefault_WhenNull_ReturnsDefault()
    {
        const string method = "MethodName";
        var arguments = new object?[] { null, "2", 3.0m };
        var value = ArgumentExistsAndIsOfTypeOrDefault<int>(method, arguments, 0);
        value.Should().Be(0);
    }

    [Fact]
    public void ArgumentsAreAllOfTypeOrDefault_WhenEmpty_ThrowsArgumentException()
    {
        const string method = "MethodName";
        var arguments = Array.Empty<object?>();
        var action = () => ArgumentsAreAllOfTypeOrDefault<string>(method, arguments);
        action.Should().Throw<ArgumentException>().WithMessage("'arguments' cannot be null or empty. (Parameter 'arguments')");
    }

    [Fact]
    public void ArgumentsAreAllOfTypeOrDefault_WhenWrongType_ThrowsArgumentException()
    {
        const string method = "MethodName";
        var arguments = new object?[] { 1, "2", 3 };
        var action = () => ArgumentsAreAllOfTypeOrDefault<int>(method, arguments);
        action.Should().Throw<ArgumentException>().WithMessage("Invalid type of arguments[1] of 'MethodName'. Expected: Int32. Found: String. (Parameter 'arguments[1]')");
    }

    [Fact]
    public void ArgumentsAreAllOfTypeOrDefault_WhenValid_ReturnsItem()
    {
        const string method = "MethodName";
        var arguments = new object?[] { null, 1, 2, 3 };
        var value = ArgumentsAreAllOfTypeOrDefault<int>(method, arguments);
        value.Should().BeOfType<int[]>().Subject.Should().BeEquivalentTo(new[] { 0, 1, 2, 3 });
    }
}
