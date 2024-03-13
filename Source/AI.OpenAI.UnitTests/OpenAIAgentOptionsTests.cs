namespace DotNetToolbox.AI.OpenAI;

public class OpenAIAgentOptionsTests {
    [Fact]
    public void DefaultConstructor_CreatesOptions() {

        // Act
        var options = new OpenAIAgentOptions();

        // Assert
        options.Model.Should().Be(DefaultModel);
        options.ApiEndpoint.Should().Be(DefaultApiEndpoint);
        options.FrequencyPenalty.Should().BeNull();
        options.PresencePenalty.Should().BeNull();
        options.MaximumOutputTokens.Should().Be(DefaultMaximumOutputTokens);
        options.NumberOfChoices.Should().BeNull();
        options.Temperature.Should().BeNull();
        options.TokenProbabilityCutOff.Should().BeNull();
        options.UseStreaming.Should().BeFalse();
        options.StopSequences.Should().BeEmpty();
        options.Tools.Should().BeEmpty();
        options.Validate().IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void ConstructorWithValues_CreatesOptions() {

        // Act
        var options = new OpenAIAgentOptions("SomeApiEndpoint", "SomeModel") {
            FrequencyPenalty = 1.5m,
            PresencePenalty = 1.1m,
            MaximumOutputTokens = 100_000,
            NumberOfChoices = 2,
            Temperature = 0.7m,
            TokenProbabilityCutOff = 0.5m,
            UseStreaming = true,
        };
        options.StopSequences.Add("Abort!");
        options.StopSequences.Add("Stop!");
        options.Tools.Add(new("MyFunction1", description: "This is my first custom function"));
        options.Tools.Add(new("MyFunction2", description: "This is my second custom function"));

        // Assert
        options.Model.Should().Be("SomeModel");
        options.ApiEndpoint.Should().Be("SomeApiEndpoint");
        options.FrequencyPenalty.Should().Be(1.5m);
        options.PresencePenalty.Should().Be(1.1m);
        options.MaximumOutputTokens.Should().Be(100_000);
        options.NumberOfChoices.Should().Be(2);
        options.Temperature.Should().Be(0.7m);
        options.TokenProbabilityCutOff.Should().Be(0.5m);
        options.UseStreaming.Should().BeTrue();
        options.StopSequences.Should().HaveCount(2);
        options.Tools.Should().HaveCount(2);
        options.Validate().IsSuccess.Should().BeTrue();
    }
}
