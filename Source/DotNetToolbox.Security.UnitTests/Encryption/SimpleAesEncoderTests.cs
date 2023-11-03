namespace DotNetToolbox.Security.Encryption;

public class SimpleAesEncoderTests {
    [Fact]
    public void Encode_AndDecode_FromString_ReturnsEncodedText() {
        // Arrange
        var key = new byte[] {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
            0, 1,
        };
        var iv = new byte[] {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
            0, 1, 2, 3, 4, 5,
        };
        const string secret = "Some secret text to encode.";
        string expected;

        using (var subject = new SimpleAesEncoder(key, iv)) {
            var result = subject.Encode(secret);

            // Act
            expected = subject.Decode(result);
        }
        // Assert
        expected.Should().Be(secret);
    }

    [Fact]
    public void Encode_AndDecode_FromBytes_ReturnsEncodedText() {
        // Arrange
        var key = new byte[] {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
            0, 1,
        };
        var iv = new byte[] {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
            0, 1, 2, 3, 4, 5,
        };
        var secret = new byte[] {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
        };
        byte[] expected;

        using (var subject = new SimpleAesEncoder(key, iv)) {
            var result = subject.Encode(secret);

            // Act
            expected = subject.Decode(result);
        }

        // Assert
        expected.Should().BeEquivalentTo(secret);
    }

    [Fact]
    public void Encode_AndDecode_AfterDispose_Throws() {
        // Arrange
        var key = new byte[] {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
            0, 1,
        };
        var iv = new byte[] {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
            0, 1, 2, 3, 4, 5,
        };
        var subject = new SimpleAesEncoder(key, iv);

        subject.Dispose();
        subject.Dispose();

        subject.Invoking(i => i.Encode("")).Should().Throw<InvalidOperationException>();
        subject.Invoking(i => i.Decode("")).Should().Throw<InvalidOperationException>();
        subject.Invoking(i => i.Encode(Array.Empty<byte>())).Should().Throw<InvalidOperationException>();
        subject.Invoking(i => i.Decode(Array.Empty<byte>())).Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Encode_AndDecode_WithNoIv_ReturnsEncodedText() {
        // Arrange
        var key = new byte[] {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
            0, 1,
        };
        var subject = new SimpleAesEncoder(key);

        subject.Dispose();
        subject.Dispose();

        subject.Invoking(i => i.Encode("")).Should().Throw<InvalidOperationException>();
        subject.Invoking(i => i.Decode("")).Should().Throw<InvalidOperationException>();
        subject.Invoking(i => i.Encode(Array.Empty<byte>())).Should().Throw<InvalidOperationException>();
        subject.Invoking(i => i.Decode(Array.Empty<byte>())).Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Constructor_WithInvalidKeySize_Throws() {
        // Arrange
        var key = new byte[] {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
        };
        var result = () => new SimpleAesEncoder(key);

        result.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_WithInvalidIvSize_Throws() {
        // Arrange
        var key = new byte[] {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
            0, 1,
        };
        var iv = new byte[] {
            0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
        };
        var result = () => new SimpleAesEncoder(key, iv);

        result.Should().Throw<ArgumentException>();
    }
}
