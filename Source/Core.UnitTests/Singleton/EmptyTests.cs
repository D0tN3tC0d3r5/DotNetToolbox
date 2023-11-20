namespace DotNetToolbox.Singleton;

public class EmptyTests {
    private record RecordWithEmpty : IEmpty<RecordWithEmpty> {
        public static RecordWithEmpty Empty => SingletonFactory<RecordWithEmpty>.Singleton;
    }

    [Fact]
    public void Static_Empty_ReturnsSingleton() {
        // Arrange & Act
        var instance1 = RecordWithEmpty.Empty;
        var instance2 = RecordWithEmpty.Empty;

        // Assert
        instance1.Should().BeOfType<RecordWithEmpty>();
        instance1.Should().BeSameAs(instance2);
    }
}