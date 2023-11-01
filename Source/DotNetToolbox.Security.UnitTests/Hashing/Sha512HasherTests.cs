namespace DotNetToolbox.Security.Hashing;

public class Sha512HasherTests {
    private readonly Sha512Hasher _sut = new();

    [Fact]
    public void Generate_ReturnsHashWithSalt() {
        // Arrange
        var secret = new byte[] { 1, 2, 3, };

        // Act
        var result = _sut.Generate(secret);

        // Assert
        _ = result.Should().NotBeNull();
        _ = result.Salt.Should().NotBeNull();
        _ = result.Value.Should().NotBeNull();
    }

    [Fact]
    public void Validate_WithInvalidHash_ReturnsFalse() {
        // Arrange
        var secret = new byte[] { 1, 2, 3, };
        var salt = new byte[] { 4, 5, 6, };
        var hash = new Hash([7, 8, 9,], salt);

        // Act
        var result = _sut.Validate(hash, secret);

        // Assert
        _ = result.Should().BeFalse();
    }

    [Fact]
    public void Validate_WithValidHash_ReturnsTrue() {
        // Arrange
        var secret = new byte[] { 1, 2, 3, };
        var subject = _sut.Generate(secret);

        // Act
        var result = _sut.Validate(subject, secret);

        // Assert
        _ = result.Should().BeTrue();
    }
}