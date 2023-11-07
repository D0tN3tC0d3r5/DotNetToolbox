namespace System.Singleton;

public class InstanceTests {
    private class ClassWithInstance : IInstance<ClassWithInstance> {
        public static ClassWithInstance Instance => SingletonFactory<ClassWithInstance>.Singleton;
    }

    [Fact]
    public void Static_Instance_ReturnsSingleton() {
        // Arrange & Act
        var instance1 = ClassWithInstance.Instance;
        var instance2 = ClassWithInstance.Instance;

        // Assert
        instance1.Should().BeOfType<ClassWithInstance>();
        instance1.Should().BeSameAs(instance2);
    }
}