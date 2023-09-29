namespace System.Extensions;

public class ValidationErrorCollectionExtensionsTests
{
    [Fact]
    public void MergeWith_ReturnsMergedList()
    {
        // Arrange
        var errors1 = new List<ValidationError> {
            new("Some message 1 {0}.", "Source 1"),
            new("Some message 2 {0}.", "Source 1"),
            new("Some message 1 {0}.", "Source 2"),
        };
        var errors2 = new List<ValidationError> {
            new("Some message 1 {0}.", "Source 1"),
            new("Some message 2 {0}.", "Source 2"),
            new("Some message 3 {0}.", "Source 2"),
        };

        // Act
        var result = errors1.Merge(errors2);

        // Assert
        result.Should().BeEquivalentTo(new ValidationError[] {
            new("Some message 1 {0}.", "Source 1"),
            new("Some message 1 {0}.", "Source 2"),
            new("Some message 2 {0}.", "Source 1"),
            new("Some message 2 {0}.", "Source 2"),
            new("Some message 3 {0}.", "Source 2"),
        });
    }

    [Fact]
    public void MergeWith_Error_ReturnsMergedList()
    {
        // Arrange
        var errors1 = new List<ValidationError> {
            new("Some message 1 {0}.", "Source 1"),
            new("Some message 2 {0}.", "Source 1"),
            new("Some message 1 {0}.", "Source 2"),
        };
        var errors2 = new ValidationError("Some message 3 {0}.", "Source 2");

        // Act
        var result = errors1.Merge(errors2);

        // Assert
        result.Should().BeEquivalentTo(new ValidationError[] {
            new("Some message 1 {0}.", "Source 1"),
            new("Some message 1 {0}.", "Source 2"),
            new("Some message 2 {0}.", "Source 1"),
            new("Some message 3 {0}.", "Source 2"),
        });
    }
}
