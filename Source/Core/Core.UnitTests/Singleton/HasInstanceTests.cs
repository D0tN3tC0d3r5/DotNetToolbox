﻿namespace DotNetToolbox.Singleton;

public class HasInstanceTests {
    private sealed class ClassWithInstance : HasInstance<ClassWithInstance>;

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
