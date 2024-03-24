namespace DotNetToolbox.AI.Chats;

public class PersonaTests {
    [Fact]
    public void DefaultConstructor_CreatesAgent() {

        // Act
        var agent = new Persona();

        // Assert
        agent.Name.Should().NotBeNull();
        agent.Description.Should().NotBeNull();
        agent.Personality.Should().BeNull();
        agent.Facts.Should().BeEmpty();
        agent.Instructions.Should().BeEmpty();
        agent.KnownTools.Should().BeEmpty();
    }

    [Fact]
    public void ConstructorWithValues_CreatesAgent() {
        // Act
        var agent = new Persona("SomeName");

        // Assert
        agent.Name.Should().Be("SomeName");
        agent.Description.Should().NotBeNull();
        agent.Personality.Should().BeNull();
        agent.Facts.Should().BeEmpty();
        agent.Instructions.Should().BeEmpty();
        agent.KnownTools.Should().BeEmpty();
    }
}
