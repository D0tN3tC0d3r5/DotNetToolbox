namespace DotNetToolbox.AI.Chats;

public class PersonaTests {
    [Fact]
    public void DefaultConstructor_CreatesAgent() {

        // Act
        var agent = new Persona();

        // Assert
        agent.Profile.Should().NotBeNull();
        agent.Skills.Should().BeEmpty();
    }

    [Fact]
    public void ConstructorWithValues_CreatesAgent() {
        // arrange
        var profile = new Profile();
        var skills = new List<Skill> { new() { Id = 1, Name = "SomeSkill" } };

        // Act
        var agent = new Persona(profile, skills);

        // Assert
        agent.Profile.Should().Be(profile);
        agent.Skills.Should().BeEquivalentTo(skills);
    }
}
