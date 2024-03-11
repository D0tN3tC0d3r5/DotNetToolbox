namespace DotNetToolbox.AI.Chats;

public class ChatTests {
    private readonly IEnvironment _environment;
    private readonly Guid _defaultGuid = Guid.NewGuid();

    public ChatTests() {
        _environment = Substitute.For<IEnvironment>();
        _environment.Guid.Returns(Substitute.For<IGuidProvider>());
        _environment.Guid.New().Returns(_defaultGuid);
    }

    [Fact]
    public void DefaultConstructor_CreatesAgent() {

        // Act
        var chat = new Chat(_environment);

        // Assert
        chat.Id.Should().Be(_defaultGuid.ToString());
        chat.Instructions.Should().NotBeNull();
        chat.Messages.Should().BeEmpty();
        chat.TotalTokens.Should().Be(0);
    }

    [Fact]
    public void ConstructorWithValues_CreatesAgent() {
        // arrange
        var instructions = new Instructions();
        var messages = new List<Message> { new("user", [new MessagePart("text", "Some message.")]) };

        // Act
        var chat = new Chat("some-id", instructions, messages);

        // Assert
        chat.Id.Should().Be("some-id");
        chat.Instructions.Should().Be(instructions);
        chat.Messages.Should().BeEquivalentTo(messages);
    }
}