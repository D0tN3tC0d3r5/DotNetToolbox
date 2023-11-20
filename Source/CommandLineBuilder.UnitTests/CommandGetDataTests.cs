namespace DotNetToolbox.CommandLineBuilder;

public class CommandGetDataTests {
    private readonly InMemoryOutputWriter _writer = new();
    private readonly Command _testCommand;

    public CommandGetDataTests() {
        _testCommand = new("test") {
            Writer = _writer
        };
    }

    [Fact]
    public void Command_GetValueOrDefault_BeforeExecute_ReturnsDefault() {
        _testCommand.Add(new Option<string>("option"));

        var value = _testCommand.GetValueOrDefault<string>("option");

        value.Should().BeNull();
    }

    [Fact]
    public async Task Command_GetValueOrDefault_WithEmptyName_Throws() {
        await _testCommand.Execute();

        var action = () => _testCommand.GetValueOrDefault<string>("");

        action.Should().Throw<ArgumentException>().WithMessage("The name cannot be null or whitespace. (Parameter 'name')");
    }

    [Fact]
    public async Task Command_GetValueOrDefault_ReturnsValue() {
        _testCommand.Add(new Option<string>("option"));
        await _testCommand.Execute("--option", "some value");

        var value = _testCommand.GetValueOrDefault<string>("option");

        value.Should().Be("some value");
    }

    [Fact]
    public async Task Command_GetValueOrDefault_ForParameter_WhenNotSet_ReturnsDefault() {
        _testCommand.Add(new Parameter<string>("option"));
        await _testCommand.Execute();

        var value = _testCommand.GetValueOrDefault<string>("option");

        value.Should().BeNull();
    }

    [Fact]
    public async Task Command_GetValueOrDefault_WithWrongType_Throws() {
        _testCommand.Add(new Option<string>("option"));
        await _testCommand.Execute("--option", "some value");

        var action = () => _testCommand.GetValueOrDefault<int>("option");

        action.Should().Throw<InvalidCastException>().WithMessage("Cannot get Int32 from option 'option' (System.String).");
    }

    [Fact]
    public async Task Command_GetValueOrDefault_ForParameter_WithWrongType_Throws() {
        _testCommand.Add(new Parameter<string>("option"));
        await _testCommand.Execute("--option", "some value");

        var action = () => _testCommand.GetValueOrDefault<int>("option");

        action.Should().Throw<InvalidCastException>().WithMessage("Cannot get Int32 from parameter 'option' (System.String).");
    }

    [Fact]
    public async Task Command_GetValueOrDefault_WhenSetByAlias_ReturnsValue() {
        _testCommand.Add(new Option<string>("option", 'o'));
        await _testCommand.Execute("-o", "some value");

        var value = _testCommand.GetValueOrDefault<string>("option");

        value.Should().Be("some value");
    }

    [Fact]
    public async Task Command_GetValueOrDefault_WhenNotSet_ReturnsDefault() {
        _testCommand.Add(new Option<string>("option"));
        await _testCommand.Execute();

        var value = _testCommand.GetValueOrDefault<string>("option");

        value.Should().BeNull();
    }

    [Fact]
    public async Task Command_GetValueOrDefault_WhenNotAdded_ReturnsDefault() {
        await _testCommand.Execute();

        var action = () => _testCommand.GetValueOrDefault<string>("option");

        action.Should().Throw<ArgumentException>().WithMessage("Argument 'option' not found. (Parameter 'nameOrAlias')");
    }

    [Fact]
    public void Command_GetValue_BeforeExecute_Throws() {
        _testCommand.Add(new Option<string>("option"));

        var action = () => _testCommand.GetValue<string>("option");

        action.Should().Throw<ArgumentException>().WithMessage("Option 'option' not set. (Parameter 'nameOrAlias')");
    }

    [Fact]
    public async Task Command_GetValue_WithEmptyName_Throws() {
        await _testCommand.Execute();

        var action = () => _testCommand.GetValue<string>("");

        action.Should().Throw<ArgumentException>().WithMessage("The name cannot be null or whitespace. (Parameter 'name')");
    }

    [Fact]
    public async Task Command_GetValue_ReturnsValue() {
        _testCommand.Add(new Option<string>("option"));
        await _testCommand.Execute("--option", "some value");

        var value = _testCommand.GetValue<string>("option");

        value.Should().Be("some value");
    }

    [Fact]
    public async Task Command_GetValue_ForParameter_ReturnsValue() {
        _testCommand.Add(new Parameter<string>("option"));
        await _testCommand.Execute("some value");

        var value = _testCommand.GetValue<string>("option");

        value.Should().Be("some value");
    }

    [Fact]
    public async Task Command_GetValue_WithWrongType_ReturnsValue() {
        _testCommand.Add(new Option<string>("option"));
        await _testCommand.Execute("--option", "some value");

        var action = () => _testCommand.GetValue<int>("option");

        action.Should().Throw<InvalidCastException>().WithMessage("Cannot get Int32 from option 'option' (System.String).");
    }

    [Fact]
    public async Task Command_GetValue_ForParameter_WithWrongType_ReturnsValue() {
        _testCommand.Add(new Parameter<string>("option"));
        await _testCommand.Execute("some value");

        var action = () => _testCommand.GetValue<int>("option");

        action.Should().Throw<InvalidCastException>().WithMessage("Cannot get Int32 from parameter 'option' (System.String).");
    }

    [Fact]
    public async Task Command_GetValue_WhenSetByAlias_ReturnsValue() {
        _testCommand.Add(new Option<string>("option", 'o'));
        await _testCommand.Execute("-o", "some value");

        var value = _testCommand.GetValue<string>("option");

        value.Should().Be("some value");
    }

    [Fact]
    public async Task Command_GetValue_WhenNotSet_Throws() {
        _testCommand.Add(new Option<string>("option"));
        await _testCommand.Execute();

        var action = () => _testCommand.GetValue<string>("option");

        action.Should().Throw<ArgumentException>().WithMessage("Option 'option' not set. (Parameter 'nameOrAlias')");
    }

    [Fact]
    public async Task Command_GetValue_ForParameter_WhenNotSet_ReturnsValue() {
        _testCommand.Add(new Parameter<string>("option"));
        await _testCommand.Execute();

        var action = () => _testCommand.GetValue<string>("option");

        action.Should().Throw<ArgumentException>().WithMessage("Parameter 'option' not set. (Parameter 'nameOrAlias')");
    }

    [Fact]
    public async Task Command_GetValue_WhenNotAdded_Throws() {
        await _testCommand.Execute();

        var action = () => _testCommand.GetValue<string>("option");

        action.Should().Throw<ArgumentException>().WithMessage("Argument 'option' not found. (Parameter 'nameOrAlias')");
    }

    [Fact]
    public void Command_GetValueOrDefault_ByIndex_BeforeExecute_ReturnsDefault() {
        _testCommand.Add(new Parameter<string>("argument"));

        var value = _testCommand.GetValueOrDefault<string>(0);

        value.Should().BeNull();
    }

    [Fact]
    public async Task Command_GetValueOrDefault_ByIndex_ReturnsValue() {
        _testCommand.Add(new Parameter<string>("argument"));
        await _testCommand.Execute("some value");

        var value = _testCommand.GetValueOrDefault<string>(0);

        value.Should().Be("some value");
    }

    [Fact]
    public async Task Command_GetValueOrDefault_ByIndex_WithIndexOutOfRange_ReturnsDefault() {
        await _testCommand.Execute();

        var action = () => _testCommand.GetValueOrDefault<string>(0);

        action.Should().Throw<ArgumentException>().WithMessage("The command contains no parameters. (Parameter 'index')");
    }

    [Fact]
    public async Task Command_GetValueOrDefault_ByIndex_WhenNotSet_ReturnsDefault() {
        _testCommand.Add(new Parameter<string>("argument"));
        await _testCommand.Execute();

        var value = _testCommand.GetValueOrDefault<string>(0);

        value.Should().BeNull();
    }

    [Fact]
    public async Task Command_GetValueOrDefault_ByIndex_WithWrongType_Throws() {
        _testCommand.Add(new Parameter<string>("argument"));
        await _testCommand.Execute("some value");

        var action = () => _testCommand.GetValueOrDefault<int>(0);

        action.Should().Throw<InvalidCastException>().WithMessage("Cannot get Int32 from parameter 'argument' (System.String).");
    }

    [Fact]
    public async Task Command_GetValue_ByIndex_ReturnsValue() {
        _testCommand.Add(new Parameter<string>("option"));
        await _testCommand.Execute("some value");

        var value = _testCommand.GetValue<string>(0);

        value.Should().Be("some value");
    }

    [Fact]
    public async Task Command_GetValue_ByIndex_WithWrongType_Throws() {
        _testCommand.Add(new Parameter<string>("option"));
        await _testCommand.Execute("some value");

        var action = () => _testCommand.GetValue<int>(0);

        action.Should().Throw<InvalidCastException>().WithMessage("Cannot get Int32 from parameter 'option' (System.String).");
    }

    [Fact]
    public async Task Command_GetValue_ByIndex_WhenNotSet_Throws() {
        _testCommand.Add(new Parameter<string>("option"));
        await _testCommand.Execute();

        var action = () => _testCommand.GetValue<string>(0);

        action.Should().Throw<ArgumentException>().WithMessage("Parameter '0' not set. (Parameter 'index')");
    }

    [Fact]
    public async Task Command_GetValue_ByIndex_WhenOutOfRange_Throws() {
        _testCommand.Add(new Parameter<string>("option"));
        await _testCommand.Execute();

        var action = () => _testCommand.GetValue<string>(2);

        action.Should().Throw<ArgumentOutOfRangeException>().WithMessage("The index is out of range (Min: 0, Max: 0). (Parameter 'index')\nActual value was 2.");
    }

    [Fact]
    public async Task Command_GetValue_ByIndex_WhenNotAdded_Throws() {
        await _testCommand.Execute();

        var action = () => _testCommand.GetValue<string>(0);

        action.Should().Throw<ArgumentException>().WithMessage("The command contains no parameters. (Parameter 'index')");
    }

    [Fact]
    public async Task Command_IsFlagSet_WithEmptyName_Throws() {
        await _testCommand.Execute();

        var action = () => _testCommand.IsFlagSet("");

        action.Should().Throw<ArgumentException>().WithMessage("The name cannot be null or whitespace. (Parameter 'name')");
    }

    [Fact]
    public async Task Command_IsFlagSet_ReturnsValue() {
        _testCommand.Add(new Flag("flag"));
        await _testCommand.Execute("--flag");

        var value = _testCommand.IsFlagSet("flag");

        value.Should().BeTrue();
    }

    [Fact]
    public async Task Command_IsFlagSet_WhenSetByAlias_ReturnsValue() {
        _testCommand.Add(new Flag("flag", 'f'));
        await _testCommand.Execute("-f");

        var value = _testCommand.IsFlagSet("flag");

        value.Should().BeTrue();
    }

    [Fact]
    public async Task Command_IsFlagSet_WhenAdded_ReturnsDefault() {
        await _testCommand.Execute();

        var action = () => _testCommand.IsFlagSet("flag");

        action.Should().Throw<ArgumentException>().WithMessage("Argument 'flag' not found. (Parameter 'nameOrAlias')");
    }

    [Fact]
    public async Task Command_IsFlagSet_WhenNotSet_ReturnsDefault() {
        _testCommand.Add(new Flag("flag"));
        await _testCommand.Execute();

        var value = _testCommand.IsFlagSet("flag");

        value.Should().BeFalse();
    }

    [Fact]
    public async Task Command_IsFlagSet_WhenWrongType_ReturnsDefault() {
        _testCommand.Add(new Option<int>("flag"));
        await _testCommand.Execute();

        var action = () => _testCommand.IsFlagSet("flag");

        action.Should().Throw<ArgumentException>().WithMessage("Argument 'flag' is not a flag. (Parameter 'nameOrAlias')");
    }

    [Fact]
    public async Task Command_GetCollectionOrDefault_WithEmptyName_Throws() {
        await _testCommand.Execute();

        var action = () => _testCommand.GetValuesOrDefault<string>("");

        action.Should().Throw<ArgumentException>().WithMessage("The name cannot be null or whitespace. (Parameter 'name')");
    }

    [Fact]
    public async Task Command_GetCollectionOrDefault_ReturnsValue() {
        _testCommand.Add(new Options<string>("option", 'o'));
        await _testCommand.Execute("--option", "value1", "-o", "value2");

        var value = _testCommand.GetValuesOrDefault<string>("option");

        value.Should().BeEquivalentTo("value1", "value2");
    }

    [Fact]
    public async Task Command_GetCollectionOrDefault_WhenNotSet_ReturnsEmpty() {
        _testCommand.Add(new Options<string>("option"));
        await _testCommand.Execute();

        var value = _testCommand.GetValuesOrDefault<string>("option");

        value.Should().BeEmpty();
    }

    [Fact]
    public async Task Command_GetCollectionOrDefault_WithWrongType_Throws() {
        _testCommand.Add(new Options<string>("option"));
        await _testCommand.Execute("--option", "value1", "-o", "value2");

        var action = () => _testCommand.GetValuesOrDefault<int>("option");

        action.Should().Throw<InvalidCastException>().WithMessage("Cannot get Int32[] from option 'option' (System.String[]).");
    }

    [Fact]
    public async Task Command_GetCollectionOrDefault_WhenNotAdded_ReturnsEmpty() {
        await _testCommand.Execute();

        var action = () => _testCommand.GetValuesOrDefault<string>("option");

        action.Should().Throw<ArgumentException>().WithMessage("Argument 'option' not found. (Parameter 'nameOrAlias')");
    }

    [Fact]
    public async Task Command_GetValues_WithEmptyName_Throws() {
        await _testCommand.Execute();

        var action = () => _testCommand.GetValues<string>("");

        action.Should().Throw<ArgumentException>().WithMessage("The name cannot be null or whitespace. (Parameter 'name')");
    }

    [Fact]
    public async Task Command_GetValues_ReturnsValue() {
        _testCommand.Add(new Options<string>("option", 'o'));
        await _testCommand.Execute("--option", "value1", "-o", "value2");

        var value = _testCommand.GetValues<string>("option");

        value.Should().BeEquivalentTo("value1", "value2");
    }

    [Fact]
    public async Task Command_GetValues_WithWrongType_Throws() {
        _testCommand.Add(new Options<string>("option"));
        await _testCommand.Execute("--option", "value1", "-o", "value2");

        var action = () => _testCommand.GetValues<int>("option");

        action.Should().Throw<InvalidCastException>().WithMessage("Cannot get Int32[] from option 'option' (System.String[]).");
    }

    [Fact]
    public async Task Command_GetValues_WhenNotSet_ReturnsEmpty() {
        _testCommand.Add(new Options<string>("option"));
        await _testCommand.Execute();

        var action = () => _testCommand.GetValues<string>("option");

        action.Should().Throw<ArgumentException>().WithMessage("Option 'option' not set. (Parameter 'nameOrAlias')");
    }

    [Fact]
    public async Task Command_GetValues_WhenNotAdd_Throws() {
        await _testCommand.Execute();

        var action = () => _testCommand.GetValues<string>("option");

        action.Should().Throw<ArgumentException>().WithMessage("Argument 'option' not found. (Parameter 'nameOrAlias')");
    }
}
