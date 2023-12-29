namespace DotNetToolbox.CommandLineBuilder;

public class CommandBuilderTests {
    [Fact]
    public void CommandBuilder_FromDefaultRoot_CreatesEmptyRootCommand() {
        var subject = CommandBuilder.FromDefaultRoot().Build();

        subject.Should().BeOfType<RootCommand>();
        subject.Name.Should().Be("testhost");
        subject.Path.Should().Be("testhost");
        subject.ToString().Should().Be("Root 'testhost'");
        subject.Tokens.Should().HaveCount(4);
        subject.Dispose();
        subject.Dispose();
    }

    [Fact]
    public void CommandBuilder_AddOption_CreatesCommandWithOption() {
        using var subject = CommandBuilder.FromDefaultRoot().AddOption<int>("option").Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Option<int>>().Subject.Name.Should().Be("option");
    }

    [Fact]
    public void CommandBuilder_AddOption_WithDescription_CreatesCommandWithOption() {
        using var subject = CommandBuilder.FromDefaultRoot().AddOption<int>("option", "Some option.").Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Option<int>>().Subject.Name.Should().Be("option");
    }

    [Fact]
    public void CommandBuilder_AddOption_WithDescription_WithAlias_CreatesCommandWithOption() {
        using var subject = CommandBuilder.FromDefaultRoot().AddOption<int>("option", 'o').Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Option<int>>().Subject.Name.Should().Be("option");
    }

    [Fact]
    public void CommandBuilder_Add_WithToken_CreatesCommandWithToken() {
        var option = new Option<int>("option");
        using var subject = CommandBuilder.FromDefaultRoot().AddChild(option).Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Option<int>>().Subject.Name.Should().Be("option");
    }

    [Fact]
    public void CommandBuilder_AddOptions_CreatesCommandWithOption() {
        using var subject = CommandBuilder.FromDefaultRoot().AddOptions<int>("option").Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Options<int>>().Subject.Name.Should().Be("option");
    }

    [Fact]
    public void CommandBuilder_AddOptions_WithDescription_CreatesCommandWithOption() {
        using var subject = CommandBuilder.FromDefaultRoot().AddOptions<int>("option", "Some option.").Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Options<int>>().Subject.Name.Should().Be("option");
    }

    [Fact]
    public void CommandBuilder_AddOptions_WithDescription_WithAlias_CreatesCommandWithOption() {
        using var subject = CommandBuilder.FromDefaultRoot().AddOptions<int>("option", 'o').Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Options<int>>().Subject.Name.Should().Be("option");
    }

    [Fact]
    public void CommandBuilder_AddFlag_CreatesCommandWithFlag() {
        using var subject = CommandBuilder.FromDefaultRoot().AddFlag("flag").Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Flag>().Subject.Name.Should().Be("flag");
    }

    [Fact]
    public void CommandBuilder_AddFlag_WithDescription_CreatesCommandWithFlag() {
        using var subject = CommandBuilder.FromDefaultRoot().AddFlag("flag", "Some flag.", true).Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Flag>().Subject.Name.Should().Be("flag");
    }

    [Fact]
    public void CommandBuilder_AddFlag_WithAlias_CreatesCommandWithFlag() {
        using var subject = CommandBuilder.FromDefaultRoot().AddFlag("flag", 'f').Build();

        subject.Tokens.Should().HaveCount(5);
        var flag = subject.Tokens[4].Should().BeOfType<Flag>().Subject;
        flag.Name.Should().Be("flag");
        flag.ValueType.Should().Be(typeof(bool));
        flag.ToString().Should().Be("Flag '--flag' | '-f'");
    }

    [Fact]
    public void CommandBuilder_AddParameter_CreatesCommandWithArgument() {
        using var subject = CommandBuilder.FromDefaultRoot().AddParameter<int>("parameter").Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Parameter<int>>().Subject.Name.Should().Be("parameter");
    }

    [Fact]
    public void CommandBuilder_AddParameter_WithDescription_CreatesCommandWithArgument() {
        using var subject = CommandBuilder.FromDefaultRoot().AddParameter<int>("parameter", "Some argument.").Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Parameter<int>>().Subject.Name.Should().Be("parameter");
    }

    [Fact]
    public void CommandBuilder_AddCommand_CreatesCommandWithSubCommand() {
        using var subject = CommandBuilder.FromDefaultRoot().AddChild<Command>("sub").Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Command>().Subject.Name.Should().Be("sub");
    }

    [Fact]
    public void CommandBuilder_AddCommand_CreatesCommandWithNameAndSubCommand() {
        using var subject = CommandBuilder.FromDefaultRoot().AddChild("sub").Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Command>().Subject.Name.Should().Be("sub");
    }

    [Fact]
    public void CommandBuilder_AddCommand_WithSetup_CreatesCommandWithSubCommand() {
        using var subject = CommandBuilder.FromDefaultRoot().AddChild("sub", b => b.AddOption<int>("option")).Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Command>().Subject.Name.Should().Be("sub");
    }

    [Fact]
    public void CommandBuilder_AddCommand_WithSetup_CreatesCommandWithNameAndSubCommand() {
        using var subject = CommandBuilder.FromDefaultRoot().AddChild<Command>("sub", b => b.AddOption<int>("option")).Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Command>().Subject.Name.Should().Be("sub");
    }

    [Fact]
    public void CommandBuilder_AddCommand_WithDescription_CreatesCommandWithSubCommand() {
        using var subject = CommandBuilder.FromDefaultRoot().AddChild("sub", "Some sub-command.").Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Command>().Subject.Name.Should().Be("sub");
    }

    [Fact]
    public void CommandBuilder_AddCommand_WithDescriptionAndSetup_CreatesCommandWithSubCommand() {
        using var subject = CommandBuilder.FromDefaultRoot().AddChild("sub", "Some sub-command.", b => b.AddOption<int>("option")).Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Command>().Subject.Name.Should().Be("sub");
    }

    [Fact]
    public void CommandBuilder_Add_WithCommand_CreatesCommandWithSubCommand() {
        var command = new Command("sub");
        using var subject = CommandBuilder.FromDefaultRoot().AddChild(command).Build();

        subject.Tokens.Should().HaveCount(5);
        subject.Tokens[4].Should().BeOfType<Command>().Subject.Name.Should().Be("sub");
    }
}
