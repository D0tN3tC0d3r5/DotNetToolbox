using TaskResponseType = DotNetToolbox.AI.Jobs.TaskResponseType;

namespace Lola.Tasks.Repositories;

public class TaskEntityTests {
    [Fact]
    public void Validate_WithValidEntity_ShouldReturnSuccess() {
        // Arrange
        var entity = new TaskEntity {
            Key = 1,
            Name = "Test Task",
            Goals = ["Goal 1"],
        };

        // Act
        var result = entity.Validate();

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithEmptyName_ShouldReturnError() {
        // Arrange
        var entity = new TaskEntity {
            Key = 1,
            Name = "",
            Goals = ["Goal 1"],
        };

        // Act
        var result = entity.Validate();

        // Assert
        result.IsInvalid.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == "The name is required.");
    }

    [Fact]
    public void Validate_WithEmptyGoals_ShouldReturnError() {
        // Arrange
        var entity = new TaskEntity {
            Key = 1,
            Name = "Test Task",
            Goals = [],
        };

        // Act
        var result = entity.Validate();

        // Assert
        result.IsInvalid.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == "At least one goal is required.");
    }

    [Fact]
    public void Validate_WithEmptyNameAndGoals_ShouldReturnMultipleErrors() {
        // Arrange
        var entity = new TaskEntity {
            Key = 1,
            Name = "",
            Goals = [],
        };

        // Act
        var result = entity.Validate();

        // Assert
        result.IsInvalid.Should().BeTrue();
        result.Errors.Should().Contain(e => e.Message == "The name is required.");
        result.Errors.Should().Contain(e => e.Message == "At least one goal is required.");
    }

    [Fact]
    public void ImplicitConversion_ToTask_ShouldConvertCorrectly() {
        // Arrange
        var entity = new TaskEntity {
            Key = 1,
            Name = "Test Task",
            Goals = ["Goal 1", "Goal 2"],
            Scope = ["Scope 1"],
            Requirements = ["Requirement 1"],
            Assumptions = ["Assumption 1"],
            Constraints = ["Constraint 1"],
            Examples = ["Example 1"],
            Guidelines = ["Guideline 1"],
            Validations = ["Validation 1"],
            InputTemplate = "Input Template",
            ResponseType = TaskResponseType.Json,
            ResponseSchema = "Response Schema",
        };

        // Act
        DotNetToolbox.AI.Jobs.Task task = entity;

        // Assert
        task.Id.Should().Be(entity.Key);
        task.Name.Should().Be(entity.Name);
        task.Goals.Should().BeEquivalentTo(entity.Goals);
        task.Scope.Should().BeEquivalentTo(entity.Scope);
        task.Requirements.Should().BeEquivalentTo(entity.Requirements);
        task.Assumptions.Should().BeEquivalentTo(entity.Assumptions);
        task.Constraints.Should().BeEquivalentTo(entity.Constraints);
        task.Examples.Should().BeEquivalentTo(entity.Examples);
        task.Guidelines.Should().BeEquivalentTo(entity.Guidelines);
        task.Validations.Should().BeEquivalentTo(entity.Validations);
        task.InputTemplate.Should().Be(entity.InputTemplate);
        task.ResponseType.Should().Be(entity.ResponseType);
        task.ResponseSchema.Should().Be(entity.ResponseSchema);
    }
}
