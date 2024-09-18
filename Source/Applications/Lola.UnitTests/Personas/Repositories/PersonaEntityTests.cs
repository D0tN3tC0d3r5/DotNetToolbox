namespace Lola.Personas.Repositories;

public class PersonaEntityTests {
    private readonly IPersonaHandler _mockPersonaHandler;
    private readonly IMap _mockContext;

    public PersonaEntityTests() {
        _mockPersonaHandler = Substitute.For<IPersonaHandler>();
        _mockContext = Substitute.For<IMap>();
        _mockContext.GetRequiredValueAs<IPersonaHandler>(nameof(PersonaHandler)).Returns(_mockPersonaHandler);
    }

    [Fact]
    public void Validate_WithValidEntity_ShouldReturnSuccess() {
        // Arrange
        var entity = new PersonaEntity {
            Key = 1,
            Name = "Test Persona",
            Role = "Test Role",
            Goals = ["Goal 1"],
        };

        _mockPersonaHandler.GetByName(entity.Name).Returns((PersonaEntity?)null);

        // Act
        var result = entity.Validate(_mockContext);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Theory]
    [InlineData(null, "The name is required.")]
    [InlineData("", "The name is required.")]
    [InlineData(" ", "The name is required.")]
    public void ValidateName_WithInvalidName_ShouldReturnError(string? name, string expectedError) {
        // Act
        var result = PersonaEntity.ValidateName(name, _mockPersonaHandler);

        // Assert
        result.IsInvalid.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == expectedError);
    }

    [Fact]
    public void ValidateName_WithExistingName_ShouldReturnError() {
        // Arrange
        const string name = "Existing Persona";
        _mockPersonaHandler.GetByName(name).Returns(new PersonaEntity());

        // Act
        var result = PersonaEntity.ValidateName(name, _mockPersonaHandler);

        // Assert
        result.IsInvalid.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == "A persona with this name is already registered.");
    }

    [Theory]
    [InlineData(null, "The role is required.")]
    [InlineData("", "The role is required.")]
    [InlineData(" ", "The role is required.")]
    public void ValidateRole_WithInvalidRole_ShouldReturnError(string? role, string expectedError) {
        // Act
        var result = PersonaEntity.ValidateRole(role);

        // Assert
        result.IsInvalid.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == expectedError);
    }

    [Fact]
    public void ValidateGoal_WithNullOrEmptyGoal_ShouldReturnError() {
        // Act
        var result = PersonaEntity.ValidateGoal(null);

        // Assert
        result.IsInvalid.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == "The goal cannot be null or empty.");
    }

    [Fact]
    public void ValidateGoals_WithEmptyList_ShouldReturnError() {
        // Arrange
        var goals = new List<string>();

        // Act
        var result = PersonaEntity.ValidateGoals(goals);

        // Assert
        result.IsInvalid.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == "At least one goal is required.");
    }

    [Fact]
    public void ValidateGoals_WithInvalidGoal_ShouldReturnError() {
        // Arrange
        var goals = new List<string> { "Valid Goal", "" };

        // Act
        var result = PersonaEntity.ValidateGoals(goals);

        // Assert
        result.IsInvalid.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == "The goal cannot be null or empty.");
    }

    [Fact]
    public void ImplicitConversion_ToMap_ShouldConvertCorrectly() {
        // Arrange
        var entity = new PersonaEntity {
            Name = "Test Persona",
            Role = "Test Role",
            Goals = ["Goal 1", "Goal 2"],
            Questions = [new() { Question = "Question 1" }],
        };

        // Act
        Map map = entity;

        // Assert
        map["Name"].Should().Be(entity.Name);
        map["Role"].Should().Be(entity.Role);
        map["Goals"].Should().BeEquivalentTo(entity.Goals);
        map["Questions"].Should().BeEquivalentTo(entity.Questions);
    }

    [Fact]
    public void ImplicitConversion_ToPersona_ShouldConvertCorrectly() {
        // Arrange
        var entity = new PersonaEntity {
            Key = 1,
            Name = "Test Persona",
            Role = "Test Role",
            Goals = ["Goal 1", "Goal 2"],
            Expertise = "Test Expertise",
            Traits = ["Trait 1", "Trait 2"],
            Important = ["Important 1"],
            Negative = ["Negative 1"],
            Other = ["Other 1"],
        };

        // Act
        DotNetToolbox.AI.Personas.Persona persona = entity;

        // Assert
        persona.Id.Should().Be(entity.Key);
        persona.Name.Should().Be(entity.Name);
        persona.Role.Should().Be(entity.Role);
        persona.Goals.Should().BeEquivalentTo(entity.Goals);
        persona.Expertise.Should().Be(entity.Expertise);
        persona.Traits.Should().BeEquivalentTo(entity.Traits);
        persona.Important.Should().BeEquivalentTo(entity.Important);
        persona.Negative.Should().BeEquivalentTo(entity.Negative);
        persona.Other.Should().BeEquivalentTo(entity.Other);
    }
}
