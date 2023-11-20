namespace DotNetToolbox.CommandLineBuilder;

public class CommandBuilderTests {
    [Fact]
    public void CommandBuilder_FromDefaultRoot_CreatesEmptyRootCommand() {
        var subject = CommandBuilder.FromDefaultRoot().Build();

        subject.Should().BeOfType<RootCommand>();
        subject.Name.Should().Be("testhost");
        subject.Path.Should().Be("testhost");
        subject.ToString().Should().Be("Command 'testhost'");
        subject.Tokens.Should().HaveCount(4);
    }

    [Fact]
    public void CommandBuilder_AddOption_CreatesCommandWithOption() {
        var subject = CommandBuilder.FromDefaultRoot().AddOption<int>("option").Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Option<int>>().Subject.Name.Should().Be("option");
    }

    [Fact]
    public void CommandBuilder_AddOption_WithDescription_CreatesCommandWithOption() {
        var subject = CommandBuilder.FromDefaultRoot().AddOption<int>("option", "Some option.").Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Option<int>>().Subject.Name.Should().Be("option");
    }

    [Fact]
    public void CommandBuilder_AddOption_WithDescription_WithAlias_CreatesCommandWithOption() {
        var subject = CommandBuilder.FromDefaultRoot().AddOption<int>("option", 'o').Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Option<int>>().Subject.Name.Should().Be("option");
    }

    [Fact]
    public void CommandBuilder_Add_WithToken_CreatesCommandWithToken() {
        var option = new Option<int>("option");
        var subject = CommandBuilder.FromDefaultRoot().Add(option).Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Option<int>>().Subject.Name.Should().Be("option");
    }

    [Fact]
    public void CommandBuilder_AddOptions_CreatesCommandWithOption() {
        var subject = CommandBuilder.FromDefaultRoot().AddOptions<int>("option").Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Options<int>>().Subject.Name.Should().Be("option");
    }

    [Fact]
    public void CommandBuilder_AddOptions_WithDescription_CreatesCommandWithOption() {
        var subject = CommandBuilder.FromDefaultRoot().AddOptions<int>("option", "Some option.").Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Options<int>>().Subject.Name.Should().Be("option");
    }

    [Fact]
    public void CommandBuilder_AddOptions_WithDescription_WithAlias_CreatesCommandWithOption() {
        var subject = CommandBuilder.FromDefaultRoot().AddOptions<int>("option", 'o').Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Options<int>>().Subject.Name.Should().Be("option");
    }

    [Fact]
    public void CommandBuilder_AddFlag_CreatesCommandWithFlag() {
        var subject = CommandBuilder.FromDefaultRoot().AddFlag("flag").Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Flag>().Subject.Name.Should().Be("flag");
    }

    [Fact]
    public void CommandBuilder_AddFlag_WithDescription_CreatesCommandWithFlag() {
        var subject = CommandBuilder.FromDefaultRoot().AddFlag("flag", "Some flag.", true).Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Flag>().Subject.Name.Should().Be("flag");
    }

    [Fact]
    public void CommandBuilder_AddFlag_WithAlias_CreatesCommandWithFlag() {
        var subject = CommandBuilder.FromDefaultRoot().AddFlag("flag", 'f').Build();

        subject.Tokens.Should().HaveCount(5);
        var flag = subject.Tokens[4].Should().BeOfType<Flag>().Subject;
        flag.Name.Should().Be("flag");
        flag.ValueType.Should().Be(typeof(bool));
        flag.ToString().Should().Be("Flag '--flag' | '-f'");
    }

    [Fact]
    public void CommandBuilder_AddParameter_CreatesCommandWithArgument() {
        var subject = CommandBuilder.FromDefaultRoot().AddParameter<int>("parameter").Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Parameter<int>>().Subject.Name.Should().Be("parameter");
    }

    [Fact]
    public void CommandBuilder_AddParameter_WithDescription_CreatesCommandWithArgument() {
        var subject = CommandBuilder.FromDefaultRoot().AddParameter<int>("parameter", "Some argument.").Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Parameter<int>>().Subject.Name.Should().Be("parameter");
    }

    [Fact]
    public void CommandBuilder_AddCommand_CreatesCommandWithSubCommand() {
        var subject = CommandBuilder.FromDefaultRoot().AddChildCommand<Command>("sub").Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Command>().Subject.Name.Should().Be("sub");
    }

    [Fact]
    public void CommandBuilder_AddCommand_WithSetup_CreatesCommandWithSubCommand() {
        var subject = CommandBuilder.FromDefaultRoot().AddChildCommand("sub", build: b => b.AddOption<int>("option")).Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Command>().Subject.Name.Should().Be("sub");
    }

    [Fact]
    public void CommandBuilder_AddCommand_WithDescription_CreatesCommandWithSubCommand() {
        var subject = CommandBuilder.FromDefaultRoot().AddChildCommand("sub", "Some sub-command.").Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Command>().Subject.Name.Should().Be("sub");
    }

    [Fact]
    public void CommandBuilder_AddCommand_WithDescriptionAndSetup_CreatesCommandWithSubCommand() {
        var subject = CommandBuilder.FromDefaultRoot().AddChildCommand("sub", "Some sub-command.", b => b.AddOption<int>("option")).Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Command>().Subject.Name.Should().Be("sub");
    }

    [Fact]
    public void CommandBuilder_Add_WithCommand_CreatesCommandWithSubCommand() {
        var command = new Command("sub");
        var subject = CommandBuilder.FromDefaultRoot().Add(command).Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Command>().Subject.Name.Should().Be("sub");
    }
}
