using System.Reflection;

namespace DotNetToolbox.Security.Attributes;
public class PersonalInformationAttributeTests {
    private class TestClass {
        public int Id { get; set; }

        [PersonalInformation]
        public string Name { get; } = string.Empty;

        [PersonalInformation(IsEncrypted = true)]
        public string CreditCardNumber { get; } = string.Empty;
    }

    [Fact]
    public void NameProperty_ShouldHavePersonalInformationAttribute_WithIsEncryptedFalse() {
        // Arrange
        var nameProperty = typeof(TestClass).GetProperty(nameof(TestClass.Name))!;

        // Act
        var nameAttribute = nameProperty.GetCustomAttribute<PersonalInformationAttribute>();

        // Assert
        nameAttribute!.IsEncrypted.Should().BeFalse();
    }


    [Fact]
    public void CreditCardNumberProperty_ShouldHavePersonalInformationAttribute_WithIsEncryptedTrue() {
        // Arrange
        var nameProperty = typeof(TestClass).GetProperty(nameof(TestClass.CreditCardNumber))!;

        // Act
        var nameAttribute = nameProperty.GetCustomAttribute<PersonalInformationAttribute>();

        // Assert
        nameAttribute!.IsEncrypted.Should().BeTrue();
    }


    [Fact]
    public void IdProperty_ShouldNotHavePersonalInformationAttribute() {
        // Arrange
        var nameProperty = typeof(TestClass).GetProperty(nameof(TestClass.Id))!;

        // Act
        var nameAttribute = nameProperty.GetCustomAttribute<PersonalInformationAttribute>();

        // Assert
        nameAttribute.Should().BeNull();
    }
}
