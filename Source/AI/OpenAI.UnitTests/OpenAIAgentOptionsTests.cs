using static DotNetToolbox.AI.Agents.AgentOptions;

namespace DotNetToolbox.AI.OpenAI;

public class OpenAIAgentOptionsTests {
    [Fact]
    public void DefaultConstructor_CreatesOptions() {

        // Act
        var options = new AgentOptions();

        // Assert
        options.Model.Should().Be(DefaultModel);
        options.ApiEndpoint.Should().Be(DefaultApiEndpoint);
        options.MaximumOutputTokens.Should().Be(DefaultMaximumOutputTokens);
        options.Temperature.Should().Be(DefaultTemperature);
        options.TokenProbabilityCutOff.Should().Be(DefaultProbabilityCutOff);
        options.UseStreaming.Should().BeFalse();
        options.JsonMode.Should().BeFalse();
        options.StopSequences.Should().BeEmpty();
        options.Validate().IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void ConstructorWithValues_CreatesOptions() {

        // Act
        var options = new AgentOptions("SomeApiEndpoint", "SomeModel") {
            MaximumOutputTokens = 100_000,
            Temperature = 0.7m,
            TokenProbabilityCutOff = 0.5m,
            UseStreaming = true,
            StopSequences  = ["Abort!", "Stop!"],
            JsonMode = true,
        };

        // Assert
        options.Model.Should().Be("SomeModel");
        options.ApiEndpoint.Should().Be("SomeApiEndpoint");
        options.MaximumOutputTokens.Should().Be(100_000);
        options.Temperature.Should().Be(0.7m);
        options.TokenProbabilityCutOff.Should().Be(0.5m);
        options.UseStreaming.Should().BeTrue();
        options.JsonMode.Should().BeTrue();
        options.StopSequences.Should().HaveCount(2);
        options.Validate().IsSuccess.Should().BeTrue();
    }
}
