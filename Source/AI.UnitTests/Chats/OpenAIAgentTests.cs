namespace DotNetToolbox.AI.Chats;

public class OpenAIAgentTests {
    [Fact]
    public void DefaultConstructor_CreatesAgent() {

        // Act
        var agent = new OpenAIAgent();

        // Assert
        agent.Options.Should().NotBeNull();
        agent.Profile.Should().NotBeNull();
        agent.Skills.Should().BeEmpty();
    }

    [Fact]
    public void ConstructorWithValues_CreatesAgent() {
        // arrange
        var options = new OpenAIAgentOptions();
        var profile = new Profile();
        var skills = new List<Skill> { new("SomeSkill") };

        // Act
        var agent = new OpenAIAgent(options, profile, skills);

        // Assert
        agent.Options.Should().Be(options);
        agent.Profile.Should().Be(profile);
        agent.Skills.Should().BeEquivalentTo(skills);
    }
}