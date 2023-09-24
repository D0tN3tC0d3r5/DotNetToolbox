namespace DotNetToolbox.Utilities;

public class CreateTests
{
    private class TestClass : IHaveSingleton<TestClass>
    {
        public static TestClass Instance => IHaveSingleton<TestClass>.Singleton;
    }

    [Fact]
    public void Static_Instance_ReturnsDefaultSingleton()
    {
        // Arrange & Act
        var instance1 = TestClass.Instance;
        var instance2 = TestClass.Instance;

        // Assert
        instance1.Should().BeOfType<TestClass>();
        instance1.Should().BeSameAs(instance2);
    }

    [Fact]
    public void Instance_NoArgs_CreatesObjectOfType()
    {
        // Arrange & Act
        var instance = Create.Instance<TestClass>();

        // Assert
        instance.Should().NotBeNull();
        instance.Should().BeOfType<TestClass>();
    }

    private class TestClassWithArgs
    {
        public string Value { get; }

        public TestClassWithArgs(string value)
        {
            Value = value;
        }
    }

    [Fact]
    public void Instance_WithArgs_CreatesObjectOfTypeAndSetsValues()
    {
        // Arrange
        const string expectedValue = "Test";

        // Act
        var instance = Create.Instance<TestClassWithArgs>(expectedValue);

        // Assert
        instance.Should().NotBeNull();
        instance.Should().BeOfType<TestClassWithArgs>();
        instance.Value.Should().Be(expectedValue);
    }
}
