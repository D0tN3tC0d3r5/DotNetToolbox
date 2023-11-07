namespace DotNetToolbox.ValidationBuilder.Commands;

public class ValidationCommandFactoryTests {
    private static object?[] Args(params object?[] args) => args;

    [Theory]
    [InlineData(typeof(decimal))]
    [InlineData(typeof(int))]
    [InlineData(typeof(string))]
    [InlineData(typeof(DateTime))]
    [InlineData(typeof(IValidatable))]
    [InlineData(typeof(List<int>))]
    [InlineData(typeof(List<string>))]
    [InlineData(typeof(Dictionary<string, int>))]
    [InlineData(typeof(Dictionary<string, decimal>))]
    [InlineData(typeof(Dictionary<string, string>))]
    [InlineData(typeof(Dictionary<double, double>))]
    public void Create_ForUnsupportedValidator_Throws(Type subjectType) {
        //Act
        var action = () => ValidationCommandFactory.For(subjectType, "Attribute").Create("Anything", []);

        //Assert
        action.Should().Throw<InvalidOperationException>();
    }

    private const string _string = "AbcDef";
    private const int _integer = 42;
    private const decimal _decimal = 42.0m;
    private static readonly DateTime _dateTime = DateTime.Parse("2020-01-01 10:10:10.12345");
    private static readonly Type _type = typeof(string);
    private static readonly List<int> _integers = [1, 2, 3,];
    private static readonly List<decimal> _decimals = [1.0m, 2.0m, 3.0m,];
    private static readonly List<int?> _nullableIntegers = [1, 2, 3,];
    private static readonly List<decimal?> _nullableDecimals = [1.0m, 2.0m, 3.0m,];
    private static readonly List<string> _strings = ["A", _string, "C",];
    private static readonly Dictionary<string, int> _strings2Integers = new() { ["A"] = 1, ["B"] = 2, ["C"] = 3, };
    private static readonly Dictionary<string, decimal> _strings2Decimals = new() { ["A"] = 1m, ["B"] = 2m, ["C"] = 3m, };
    private static readonly Dictionary<string, int?> _strings2NullableIntegers = new() { ["A"] = 1, ["B"] = 2, ["C"] = 3, };
    private static readonly Dictionary<string, decimal?> _strings2NullableDecimals = new() { ["A"] = 1m, ["B"] = 2m, ["C"] = 3m, };
    private static readonly Dictionary<string, string> _strings2Strings = new() { ["A"] = "1", ["B"] = "2", ["C"] = "3", };

    private class TestDataForValidateSuccess : TheoryData<string, object?[], object?, Type> {
        public TestDataForValidateSuccess() {
            Add(CommandNames.Contains, Args(_integers[1]), _integers, _integers.GetType());
            Add(CommandNames.Contains, Args(_string[1..^1]), _string, _string.GetType());
            Add(CommandNames.ContainsKey, Args(_strings2Strings.Keys.First()), _strings2Strings, _strings2Strings.GetType());
            Add(CommandNames.ContainsValue, Args(_strings2Integers.Values.Last()), _strings2Integers, _strings2Integers.GetType());
            Add(CommandNames.Has, Args(_integers.Count), _integers, _integers.GetType());
            Add(CommandNames.Has, Args(_decimals.Count), _decimals, _decimals.GetType());
            Add(CommandNames.Has, Args(_nullableIntegers.Count), _integers, _nullableIntegers.GetType());
            Add(CommandNames.Has, Args(_nullableDecimals.Count), _decimals, _nullableDecimals.GetType());
            Add(CommandNames.Has, Args(_strings.Count), _strings, _strings.GetType());
            Add(CommandNames.Has, Args(_strings2Integers.Count), _strings2Integers, _strings2Integers.GetType());
            Add(CommandNames.Has, Args(_strings2Decimals.Count), _strings2Decimals, _strings2Decimals.GetType());
            Add(CommandNames.Has, Args(_strings2NullableIntegers.Count), _strings2Integers, _strings2NullableIntegers.GetType());
            Add(CommandNames.Has, Args(_strings2NullableDecimals.Count), _strings2Decimals, _strings2NullableDecimals.GetType());
            Add(CommandNames.Has, Args(_strings2Strings.Count), _strings2Strings, _strings2Strings.GetType());
            Add(CommandNames.HasAtLeast, Args(_strings.Count), _strings, _strings.GetType());
            Add(CommandNames.HasAtLeast, Args(_strings2Strings.Count), _strings2Strings, _strings2Strings.GetType());
            Add(CommandNames.HasAtMost, Args(_strings.Count), _strings, _strings.GetType());
            Add(CommandNames.HasAtMost, Args(_strings2Strings.Count), _strings2Strings, _strings2Strings.GetType());
            Add(CommandNames.IsAfter, Args(_dateTime), _dateTime.AddSeconds(1), _dateTime.GetType());
            Add(CommandNames.IsBefore, Args(_dateTime), _dateTime.AddSeconds(-1), _dateTime.GetType());
            Add(CommandNames.IsEmpty, Args(), _integers.Where(_ => false).ToList(), _integers.GetType());
            Add(CommandNames.IsEmpty, Args(), _strings2Integers.Where(_ => false).ToDictionary(k => k.Key, v => v.Value), _strings2Integers.GetType());
            Add(CommandNames.IsEqualTo, Args(_dateTime), _dateTime, _dateTime.GetType());
            Add(CommandNames.IsEqualTo, Args(_decimal), _decimal, _decimal.GetType());
            Add(CommandNames.IsEqualTo, Args(_integer), _integer, _integer.GetType());
            Add(CommandNames.IsEqualTo, Args(_integers), new List<int> { 1, 3, 2, }, _integers.GetType());
            Add(CommandNames.IsEqualTo, Args(_string), _string, _string.GetType());
            Add(CommandNames.IsEqualTo, Args(_strings), _strings, _strings.GetType());
            Add(CommandNames.IsEqualTo, Args(_type), _type, typeof(Type));
            Add(CommandNames.IsGreaterThan, Args(_decimal), _decimal + 0.01m, _decimal.GetType());
            Add(CommandNames.IsGreaterThan, Args(_integer), _integer + 1, _integer.GetType());
            Add(CommandNames.IsIn, Args(_strings.OfType<object?>().ToArray()), _string, _string.GetType());
            Add(CommandNames.IsLessThan, Args(_decimal), _decimal - 0.01m, _decimal.GetType());
            Add(CommandNames.IsLessThan, Args(_integer), _integer - 1, _integer.GetType());
            Add(CommandNames.IsNull, Args(), default(string), typeof(string));
            Add(CommandNames.LengthIs, Args(_string.Length), _string, _string.GetType());
            Add(CommandNames.LengthIsAtLeast, Args(_string.Length), _string, _string.GetType());
            Add(CommandNames.LengthIsAtMost, Args(_string.Length), _string, _string.GetType());
        }
    }

    [Theory]
    [ClassData(typeof(TestDataForValidateSuccess))]
    public void Validate_WithValidSubject_ReturnsSuccess(string validatorName, object?[] args, object? validValue, Type valueType) {
        var validator = ValidationCommandFactory.For(valueType, "Attribute").Create(validatorName, args);
        var validResult = validator.Validate(validValue);

        validResult.IsSuccess.Should().BeTrue();
    }

    private class TestDataForValidateFailure : TheoryData<string, object?[], object?, Type> {
        public TestDataForValidateFailure() {
            Add(CommandNames.IsNull, Args(), "NotNull", typeof(string));
            Add(CommandNames.IsEmpty, Args(), _integers, _integers.GetType());
            Add(CommandNames.IsEmpty, Args(), _strings2Integers, _strings2Integers.GetType());
            Add(CommandNames.Contains, Args("Xyz"), _string, _string.GetType());
            Add(CommandNames.Contains, Args(13), _integers, _integers.GetType());
            Add(CommandNames.ContainsKey, Args("Nope"), _strings2Strings, _strings2Strings.GetType());
            Add(CommandNames.ContainsValue, Args(13), _strings2Integers, _strings2Integers.GetType());
            Add(CommandNames.Has, Args(99), _integers, _integers.GetType());
            Add(CommandNames.Has, Args(99), _strings, _strings.GetType());
            Add(CommandNames.Has, Args(99), _strings2Decimals, _strings2Decimals.GetType());
            Add(CommandNames.Has, Args(99), _strings2Integers, _strings2Integers.GetType());
            Add(CommandNames.Has, Args(99), _strings2Strings, _strings2Strings.GetType());
            Add(CommandNames.IsAfter, Args(_dateTime), _dateTime.AddSeconds(-1), _dateTime.GetType());
            Add(CommandNames.IsBefore, Args(_dateTime), _dateTime.AddSeconds(1), _dateTime.GetType());
            Add(CommandNames.IsEqualTo, Args(_integer), _integers, _integers.GetType());
            Add(CommandNames.IsEqualTo, Args(_integers), _integer, _integer.GetType());
            Add(CommandNames.IsEqualTo, Args(_integers), new List<int> { 1, 2, }, _integers.GetType());
            Add(CommandNames.IsEqualTo, Args(_integers), new List<int> { 1, 2, 4, }, _integers.GetType());
            Add(CommandNames.IsEqualTo, Args(_integers), new List<int> { 1, 2, 3, 3, }, _integers.GetType());
            Add(CommandNames.IsEqualTo, Args(new List<int> { 1, 3, 2, 2, }), new List<int> { 1, 2, 3, 3, }, _integers.GetType());
            Add(CommandNames.IsEqualTo, Args(_integers), new List<string> { "1", "2", "2", }, _integers.GetType());
            Add(CommandNames.IsEqualTo, Args(_string), "Nope", _string.GetType());
            Add(CommandNames.IsEqualTo, Args(_integer), 13, _integer.GetType());
            Add(CommandNames.IsEqualTo, Args(_decimal), 13.01m, _decimal.GetType());
            Add(CommandNames.IsEqualTo, Args(_dateTime), _dateTime.AddDays(-1), _dateTime.GetType());
            Add(CommandNames.IsEqualTo, Args(_type), typeof(long), typeof(Type));
            Add(CommandNames.IsGreaterThan, Args(_integer), _integer - 1, _integer.GetType());
            Add(CommandNames.IsGreaterThan, Args(_decimal), _decimal - 0.01m, _decimal.GetType());
            Add(CommandNames.IsLessThan, Args(_decimal), _decimal + 0.01m, _decimal.GetType());
            Add(CommandNames.IsLessThan, Args(_integer), _integer + 1, _integer.GetType());
            Add(CommandNames.IsIn, Args(_strings.OfType<object?>().ToArray()), "Nope", _string.GetType());
            Add(CommandNames.LengthIs, Args(2), _string, _string.GetType());
            Add(CommandNames.HasAtMost, Args(2), _strings2Strings, _strings2Strings.GetType());
            Add(CommandNames.HasAtMost, Args(2), _strings, _strings.GetType());
            Add(CommandNames.LengthIsAtMost, Args(2), _string, _string.GetType());
            Add(CommandNames.HasAtLeast, Args(99), _strings2Strings, _strings2Strings.GetType());
            Add(CommandNames.HasAtLeast, Args(99), _strings, _strings.GetType());
            Add(CommandNames.LengthIsAtLeast, Args(99), _string, _string.GetType());
        }
    }

    [Theory]
    [ClassData(typeof(TestDataForValidateFailure))]
    public void Validate_WithInvalidSubject_ReturnsFailure(
        string validatorName,
        object?[] args,
        object invalidValue,
        Type valueType) {
        var validator = ValidationCommandFactory.For(valueType, "Attribute").Create(validatorName, args);
        var invalidResult = validator.Validate(invalidValue);

        invalidResult.IsInvalid.Should().BeTrue();
    }

    private class TestCommand() : ValidationCommand("Source");

    [Fact]
    public void Validate_WithDefaultCommand_ReturnsSuccess() {
        var command = new TestCommand();

        var result = command.Validate("Value");

        result.IsSuccess.Should().BeTrue();
    }
}