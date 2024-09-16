namespace Lola.Tasks.Repositories;

public class TaskDataSourceTests {
    [Fact]
    public void Constructor_ShouldInitializeCorrectly() {
        // Arrange
        var mockStorage = Substitute.For<ITaskStorage>();

        // Act
        var subject = new TaskDataSource(mockStorage);

        // Assert
        subject.Should().BeAssignableTo<ITaskDataSource>();
        subject.Should().BeAssignableTo<DataSource<ITaskStorage, TaskEntity, uint>>();
    }
}
