using static DotNetToolbox.AI.Agents.AgentOptions;

namespace DotNetToolbox.AI.OpenAI;

public class OpenAIAgentOptionsTests {
    [Fact]
    public void DefaultConstructor_CreatesOptions() {

        // Act
        var options = new AgentOptions();

        // Assert
        options.Model.Should().BeEmpty();
        options.ChatEndpoint.Should().BeEmpty();
        options.MaximumOutputTokens.Should().Be(DefaultMaximumOutputTokens);
        options.Temperature.Should().Be(DefaultTemperature);
        options.TokenProbabilityCutOff.Should().Be(DefaultProbabilityCutOff);
        options.UseStreaming.Should().BeFalse();
        options.JsonMode.Should().BeFalse();
        options.StopSequences.Should().BeEmpty();
        options.Validate().IsSuccess.Should().BeTrue();
    }
}
