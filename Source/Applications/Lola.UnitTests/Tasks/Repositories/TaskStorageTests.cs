namespace Lola.Tasks.Repositories;

public class TaskStorageTests {
    [Fact]
    public void Constructor_ShouldInitializeCorrectly() {
        // Arrange
        var mockConfiguration = Substitute.For<IConfiguration>();

        // Act
        var subject = new TaskStorage(mockConfiguration);

        // Assert
        subject.Should().BeAssignableTo<ITaskStorage>();
        subject.Should().BeAssignableTo<JsonFilePerTypeStorage<TaskEntity, uint>>();
    }

    [Fact]
    public void Add_ShouldAssignIncrementalIds() {
        // Arrange
        var mockConfiguration = Substitute.For<IConfiguration>();
        var taskHandler = Substitute.For<ITaskHandler>();
        var context = new Map {
            [nameof(EntityAction)] = EntityAction.Insert,
            [nameof(TaskHandler)] = taskHandler,
        };
        var subject = new TaskStorage(mockConfiguration);
        var entity1 = new TaskEntity { Name = "Alpha", Goals = ["Some goal."] };
        var entity2 = new TaskEntity { Name = "Bravo", Goals = ["Some goal."] };

        // Act
        subject.Add(entity1, context);
        subject.Add(entity2, context);

        // Assert
        entity1.Id.Should().Be(1u);
        entity2.Id.Should().Be(2u);
    }

    [Fact]
    public void GetAll_ShouldReturnEntitiesWithCorrectIds() {
        // Arrange
        var mockConfiguration = Substitute.For<IConfiguration>();
        var taskHandler = Substitute.For<ITaskHandler>();
        var context = new Map {
            [nameof(EntityAction)] = EntityAction.Insert,
            [nameof(TaskHandler)] = taskHandler,
        };
        var subject = new TaskStorage(mockConfiguration);
        var entity1 = new TaskEntity { Name = "Alpha", Goals = ["Some goal."] };
        var entity2 = new TaskEntity { Name = "Bravo", Goals = ["Some goal."] };

        // Act
        subject.Add(entity1, context);
        subject.Add(entity2, context);
        var allEntities = subject.GetAll().ToList();

        // Assert
        allEntities.Should().HaveCount(2);
        allEntities[0].Id.Should().Be(1u);
        allEntities[1].Id.Should().Be(2u);
    }
}
