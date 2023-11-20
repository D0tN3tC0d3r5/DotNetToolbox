namespace DotNetToolbox.CommandLineBuilder;

public class CommandConfigurationTests {
    [Fact]
    public void Command_AddOption_AddsOption() {
        var subject = new Command("command");

        subject.Add(new Option<string>("option", 'o', description: "Option description."));

        subject.Tokens.Should().HaveCount(2).And.Contain(i => i.Name == "option");
    }

    [Fact]
    public void Command_AddOption_WithEmptyName_Throws() {
        var subject = new Command("command");

        var action = () => subject.Add(new Option<string>(""));

        action.Should().Throw<ArgumentException>().WithMessage("The name cannot be null or whitespace. (Parameter 'name')");
    }

    [Fact]
    public void Command_AddOption_WithEmptyAlias_Throws() {
        var subject = new Command("command");

        var action = () => subject.Add(new Option<string>("option", '!'));

        action.Should().Throw<ArgumentException>().WithMessage("'!' is not a valid alias. An alias must be a letter or number. (Parameter 'alias')");
    }

    [Fact]
    public void Command_AddOption_InvalidEmptyName_Throws() {
        var subject = new Command("command");

        var action = () => subject.Add(new Option<string>("!123"));

        action.Should().Throw<ArgumentException>().WithMessage("'!123' is not a valid name. A name must be in the 'kebab case' form. Examples: 'name', 'address2' or 'full-name'. (Parameter 'name')");
    }

    [Fact]
    public void Command_AddFlag_WithDuplicatedName_Throws()
    {
        var subject = new Command("command");
        subject.Add(new Flag("flag", 'f', description: "Flag description."));

        var action = () => subject.Add(new Flag("flag"));

        action.Should().Throw<InvalidOperationException>().WithMessage("An argument with name 'flag' already exists.");
    }

    [Fact]
    public void Command_AddFlag_WithDuplicatedAlias_Throws()
    {
        var subject = new Command("command");
        subject.Add(new Flag("flag", 'f', description: "Flag description."));

        var action = () => subject.Add(new Flag("first", 'f'));

        action.Should().Throw<InvalidOperationException>().WithMessage("An argument with alias 'f' already exists.");
    }

    [Fact]
    public void Command_AddMultiOption_AddsOption() {
        var subject = new Command("command");

        subject.Add(new Options<string>("option", 'o', "Option description."));

        subject.Tokens.Should().HaveCount(2).And.Contain(i => i.Name == "option");
    }

    [Fact]
    public void Command_AddFlag_WithFlag_AddsOption() {
        var subject = new Command("command");

        subject.Add(new Flag("flag", 'f', "Flag description."));

        subject.Tokens.Should().HaveCount(2).And.Contain(i => i.Name == "flag");
    }

    [Fact]
    public void Command_AddParameter_AddsParameter() {
        var subject = new Command("command");

        subject.Add(new Parameter<int>("parameter", "Parameter description."));

        subject.Tokens.Should().HaveCount(2).And.Contain(i => i.Name == "parameter");
    }

    [Fact]
    public void Command_AddCommand_AddsSubCommand() {
        var subject = new Command("command");

        subject.Add(new Command("sub-command", "Sub command description."));

        subject.Tokens.Should().HaveCount(2).And.Contain(i => i.Name == "sub-command");
    }
}
