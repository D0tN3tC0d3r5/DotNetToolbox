using System.Text;

using FluentAssertions.Extensions;

namespace DotNetToolbox.Security.Hashing;

public class Sha512HasherTests {
    private readonly Sha512Hasher _subject = new();

    [Fact]
    public void Constants_ReturnsDefaultValues()
        => Sha512Hasher.DefaultIterations.Should().Be(259733);

    [Fact]
    public void ConstructorWithIterations_Generate_ReturnsHashWithSalt() {
        // Arrange
        var subject = new Sha512Hasher(1000);
        var secret = new byte[] { 1, 2, 3, };

        // Act
        var result = subject.Generate(secret);

        // Assert
        result.Should().NotBeNull();
        result.Salt.Should().NotBeNull();
        result.Value.Should().NotBeNull();
    }

    [Fact]
    public void ConstructorWithInvalidIterations_Throws() {
        // Arrange & Act
        var result = () => new Sha512Hasher(0);

        // Assert
        result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Generate_ReturnsHashWithSalt() {
        // Arrange
        var secret = new byte[] { 1, 2, 3, };

        // Act
        var result = _subject.Generate(secret);

        // Assert
        result.Should().NotBeNull();
        result.Salt.Should().NotBeNull();
        result.Value.Should().NotBeNull();
    }

    [Fact]
    public void Generate_FromString_ReturnsHashWithSalt() {
        // Arrange
        const string secret = "SomePassword";

        // Act
        var result = _subject.Generate(secret);

        // Assert
        result.Should().NotBeNull();
        result.Salt.Should().NotBeNull();
        result.Value.Should().NotBeNull();
    }

    [Fact]
    public void Validate_WithInvalidHash_ReturnsFalse() {
        // Arrange
        var secret = new byte[] { 1, 2, 3, };
        var salt = new byte[] { 4, 5, 6, };
        var hash = new Hash([7, 8, 9,], salt);

        // Act
        var result = _subject.Validate(hash, secret);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithValidHash_ReturnsTrue() {
        // Arrange
        var secret = new byte[] { 1, 2, 3, };
        var subject = _subject.Generate(secret);

        // Act
        var result = _subject.Validate(subject, secret);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Validate_FromString_WithValidHash_ReturnsTrue() {
        // Arrange
        const string secret = "SomePassword";
        var subject = _subject.Generate(secret);

        // Act
        var result = _subject.Validate(subject, secret);

        // Assert
        result.Should().BeTrue();
    }
}
