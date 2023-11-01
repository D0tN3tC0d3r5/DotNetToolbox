namespace System.Singleton;

public class DefaultTests {
    private class ClassWithDefault : IDefault<ClassWithDefault> {
        public static ClassWithDefault Default => SingletonFactory<ClassWithDefault>.Singleton;
    }

    [Fact]
    public void Static_Default_ReturnsSingleton() {
        // Arrange & Act
        var instance1 = ClassWithDefault.Default;
        var instance2 = ClassWithDefault.Default;

        // Assert
        _ = instance1.Should().BeOfType<ClassWithDefault>();
        _ = instance1.Should().BeSameAs(instance2);
    }
}