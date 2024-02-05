namespace DotNetToolbox.Results;

public class ValidationErrorsTests {
    [Fact]
    public void DefaultConstructor_ShouldCreateEmptyCollection() {
        // Arrange & Act
        // ReSharper disable once CollectionNeverUpdated.Local
        var errors = new ValidationErrors();

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void Constructor_WithIEnumerable_ShouldCreateCollectionWithDistinctErrors() {
        // Arrange
        var errorList = new List<ValidationError>
        {
            new("Error1"),
            new("Error1"),
            new("Error2"),
        };

        // Act
        var errors = new ValidationErrors(errorList);

        // Assert
        errors.Should().HaveCount(2);
        errors.Should().Contain(new ValidationError("Error1"));
        errors.Should().Contain(new ValidationError("Error2"));
    }

    [Fact]
    public void ImplicitConversion_FromString_ShouldCreateCollectionWithSingleError() {
        // Arrange & Act
        ValidationErrors errors = "Error1";

        // Assert
        errors.Should().ContainSingle(e => e.Message == "Error1");
    }

    [Fact]
    public void ImplicitConversion_ToList_ShouldCreateCollectionWithSingleError() {
        // Arrange & Act
        ValidationErrors errors = "Error1";

        // Act
        List<ValidationError> list = errors;

        // Assert
        list.Should().ContainSingle(e => e.Message == "Error1");
    }

    [Fact]
    public void ImplicitConversion_ToHashSetString_ShouldCreateCollectionWithSingleError() {
        // Arrange & Act
        ValidationErrors errors = "Error1";

        // Act
        HashSet<ValidationError> set = errors;

        // Assert
        set.Should().ContainSingle(e => e.Message == "Error1");
    }

    [Fact]
    public void AdditionOperator_ShouldCombineDistinctErrorsFromTwoCollections() {
        // Arrange
        ValidationErrors errors1 = new ValidationError("Error1");
        ValidationErrors errors2 = new ValidationError("Error2");

        // Act
        var combinedErrors = errors1 + errors2;

        // Assert
        combinedErrors.Should().HaveCount(2);
        combinedErrors.Should().Contain(new ValidationError("Error1"));
        combinedErrors.Should().Contain(new ValidationError("Error2"));
    }

    [Fact]
    public void Add_ShouldAddErrorIfNotAlreadyPresent() {
        // Arrange
        var errors = new ValidationErrors();
        var error = new ValidationError("Error1");

        // Act
        errors.Add(error);

        // Assert
        errors.Should().ContainSingle(e => e.Message == "Error1");
    }

    [Fact]
    public void IListAdd_ShouldAddErrorIfNotAlreadyPresent() {
        // Arrange
        IList errors = new ValidationErrors();
        object error = new ValidationError("Error1");

        // Act
        errors.Add(error);

        // Assert
        errors.Cast<ValidationError>().Should().ContainSingle(e => e.Message == "Error1");
    }

    [Fact]
    public void IReadOnlyListIndexer_ShouldReturnCorrectError() {
        // Arrange
        IReadOnlyList<ValidationError> errors = new ValidationErrors(new[] { new ValidationError("Error1") });

        // Act
        var error = errors[0];

        // Assert
        error.Message.Should().Be("Error1");
    }

    [Fact]
    public void ImplicitConversion_FromValidationErrorArray_ShouldCreateCollectionWithDistinctErrors() {
        // Arrange
        // Act
        ValidationErrors errors = new[] {
            new ValidationError("Error1"),
            new ValidationError("Error1"),
        };

        // Assert
        errors.Should().ContainSingle();
        errors.Should().Contain(new ValidationError("Error1"));
    }

    [Fact]
    public void ImplicitConversion_ToValidationErrorArray_ShouldReturnArrayWithAllErrors() {
        // Arrange
        var errors = new ValidationErrors(new[] {
            new ValidationError("Error1"),
            new ValidationError("Error2"),
        });

        // Act
        ValidationError[] errorArray = errors;

        // Assert
        errorArray.Should().BeEquivalentTo(errors);
    }

    [Fact]
    public void AdditionOperator_WithSingleValidationError_ShouldAddErrorIfNotPresent() {
        // Arrange
        var errors = new ValidationErrors();
        var error = new ValidationError("Error1");

        // Act
        errors += error;

        // Assert
        errors.Should().ContainSingle(e => e.Message == "Error1");
    }

    [Fact]
    public void Indexer_GetterAndSetter_ShouldGetAndSetCorrectly() {
        // Arrange
        var errors = new ValidationErrors();
        var error = new ValidationError("Error1");
        errors.Add(error);

        // Act
        var retrievedError = errors[0];
        errors[0] = new("Error2");
        var updatedError = errors[0];

        // Assert
        retrievedError.Should().Be(error);
        updatedError.Message.Should().Be("Error2");
    }

    [Fact]
    public void IListAdd_WithNonValidationError_ShouldThrowArgumentException() {
        // Arrange
        // ReSharper disable once CollectionNeverQueried.Local
        IList errors = new ValidationErrors();

        // Act
        Action act = () => errors.Add("Not a ValidationError");

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Add_WithDuplicateError_ShouldNotChangeCollection() {
        // Arrange
        var errors = new ValidationErrors();
        var error = new ValidationError("Error1");
        errors.Add(error);

        // Act
        errors.Add(error);

        // Assert
        errors.Should().ContainSingle();
    }

    [Fact]
    public void Insert_AddsItem() {
        // Arrange
        // ReSharper disable once CollectionNeverQueried.Local
        var errors = new ValidationErrors();
        var error1 = new ValidationError("Error1");
        var error2 = new ValidationError("Error2");
        errors.Add(error1);

        // Act
        errors.Insert(0, error2);

        // Assert
        errors[0].Should().BeEquivalentTo(error2);
        errors[1].Should().BeEquivalentTo(error1);
    }

    [Fact]
    public void Insert_WithDuplicateErrorAtSameIndex_ShouldNotThrow() {
        // Arrange
        // ReSharper disable once CollectionNeverQueried.Local
        var errors = new ValidationErrors();
        var error1 = new ValidationError("Error1");
        var error2 = new ValidationError("Error2");
        errors.Add(error1);
        errors.Add(error2);

        // Act
        var act1 = () => errors.Insert(0, error1);

        // Assert
        act1.Should().NotThrow();
    }

    [Fact]
    public void Insert_WithDuplicateErrorAtDifferentIndex_ShouldThrow() {
        // Arrange
        // ReSharper disable once CollectionNeverQueried.Local
        var errors = new ValidationErrors();
        var error1 = new ValidationError("Error1");
        var error2 = new ValidationError("Error2");
        errors.Add(error1);
        errors.Add(error2);

        // Act
        var act1 = () => errors.Insert(1, error1);

        // Assert
        act1.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Remove_RemovesItem() {
        // Arrange
        // ReSharper disable once CollectionNeverQueried.Local
        var errors = new ValidationErrors();
        var error1 = new ValidationError("Error1");
        var error2 = new ValidationError("Error2");
        errors.Add(error1);
        errors.Add(error2);

        // Act
        errors.Remove(error1);

        // Assert
        errors.Should().ContainSingle();
        errors[0].Should().BeEquivalentTo(error2);
    }

    [Fact]
    public void RemoveAt_RemovesItem() {
        // Arrange
        // ReSharper disable once CollectionNeverQueried.Local
        var errors = new ValidationErrors();
        var error1 = new ValidationError("Error1");
        var error2 = new ValidationError("Error2");
        errors.Add(error1);
        errors.Add(error2);

        // Act
        errors.RemoveAt(0);

        // Assert
        errors.Should().ContainSingle();
        errors[0].Should().BeEquivalentTo(error2);
    }

    [Fact]
    public void Clear_RemovesAllItems() {
        // Arrange
        // ReSharper disable once CollectionNeverQueried.Local
        var errors = new ValidationErrors();
        var error1 = new ValidationError("Error1");
        var error2 = new ValidationError("Error2");
        errors.Add(error1);
        errors.Add(error2);

        // Act
        errors.Clear();

        // Assert
        errors.Should().BeEmpty();
    }
    [Fact]
    public void Add_WithString_ShouldCreateValidationErrorAndAddIfNotPresent() {
        // Arrange
        #pragma warning disable IDE0028
        var errors = new ValidationErrors();
        #pragma warning restore IDE0028

        // Act
        errors.Add("Error1");

        // Assert
        errors.Should().ContainSingle(e => e.Message == "Error1");
    }

    [Fact]
    public void Contains_WithString_ShouldReturnTrueIfErrorPresent() {
        // Arrange
        var errors = new ValidationErrors { new ValidationError("Error1") };

        // Act
        var contains = errors.Contains("Error1");

        // Assert
        contains.Should().BeTrue();
    }

    [Fact]
    public void IndexOf_WithString_ShouldReturnCorrectIndexIfErrorPresent() {
        // Arrange
        var errors = new ValidationErrors { "Error1" };

        // Act
        var index = errors.IndexOf("Error1");

        // Assert
        index.Should().Be(0);
    }

    [Fact]
    public void Insert_WithString_ShouldCreateValidationErrorAndInsertAtSpecifiedIndex() {
        // Arrange
        var errors = new ValidationErrors { "Error1" };

        // Act
        errors.Insert(0, "Error2");

        // Assert
        errors.IndexOf("Error2").Should().Be(0);
    }

    [Fact]
    public void Remove_WithString_ShouldRemoveValidationErrorIfPresent() {
        // Arrange
        var errors = new ValidationErrors { "Error1" };

        // Act
        errors.Remove("Error1");

        // Assert
        errors.Should().BeEmpty();
    }

    // Tests for explicit interface implementations
    [Fact]
    public void IList_Contains_WithValidationError_ShouldReturnTrueIfPresent() {
        // Arrange
        IList errors = new ValidationErrors();
        var error = new ValidationError("Error1");
        errors.Add(error);

        // Act
        var contains = errors.Contains(error);

        // Assert
        contains.Should().BeTrue();
    }

    [Fact]
    public void IList_IndexOf_WithValidationError_ShouldReturnCorrectIndex() {
        // Arrange
        IList errors = new ValidationErrors();
        var error = new ValidationError("Error1");
        errors.Add(error);

        // Act
        var index = errors.IndexOf(error);

        // Assert
        index.Should().Be(0);
    }

    [Fact]
    public void IList_Insert_WithValidationError_ShouldInsertAtSpecifiedIndex() {
        // Arrange
        IList errors = new ValidationErrors();
        var error = new ValidationError("Error1");

        // Act
        errors.Insert(0, error);

        // Assert
        errors[0].Should().Be(error);
    }

    [Fact]
    public void IList_Remove_WithValidationError_ShouldRemoveIfPresent() {
        // Arrange
        IList errors = new ValidationErrors();
        var error = new ValidationError("Error1");
        errors.Add(error);

        // Act
        errors.Remove(error);

        // Assert
        errors.Cast<ValidationError>().Should().BeEmpty();
    }

    [Fact]
    public void ICollection_IsSynchronized_ShouldReturnFalse() {
        // Arrange
        // ReSharper disable once CollectionNeverUpdated.Local
        ICollection errors = new ValidationErrors();

        // Act & Assert
        errors.IsSynchronized.Should().BeFalse();
    }

    [Fact]
    public void ICollection_SyncRoot_ShouldNotBeNull() {
        // Arrange
        // ReSharper disable once CollectionNeverUpdated.Local
        ICollection errors = new ValidationErrors();

        // Act & Assert
        errors.SyncRoot.Should().NotBeNull();
    }

    [Fact]
    public void IList_SetterAtIndex_ShouldUpdateValidationError() {
        // Arrange
        #pragma warning disable IDE0028
        IList errors = new ValidationErrors();
        errors.Add(new ValidationError("Error1"));
        #pragma warning restore IDE0028
        var newError = new ValidationError("Error2");

        // Act
        errors[0] = newError;

        // Assert
        errors[0].Should().Be(newError);
    }

    [Fact]
    public void IList_SetterAtIndex_WithString_ShouldUpdateValidationError() {
        // Arrange
        #pragma warning disable IDE0028
        IList errors = new ValidationErrors();
        errors.Add(new ValidationError("Error1"));
        #pragma warning restore IDE0028

        // Act
        errors[0] = "Error2";

        // Assert
        errors.Cast<ValidationError>().ToArray()[0].Message.Should().Be("Error2");
    }

    [Fact]
    public void CopyTo_NonGenericArray_ShouldCopyAllElementsToSpecifiedArrayIndex() {
        // Arrange
        var errors = new ValidationErrors(new[] { new ValidationError("Error1"), new ValidationError("Error2") });
        Array array = new ValidationError[3];

        // Act
        errors.CopyTo(array, 1);

        // Assert
        array.GetValue(1).Should().BeEquivalentTo(new ValidationError("Error1"));
        array.GetValue(2).Should().BeEquivalentTo(new ValidationError("Error2"));
    }

    [Fact]
    public void IsReadOnly_ShouldReturnFalse() {
        // Arrange
        // ReSharper disable once CollectionNeverUpdated.Local
        var errors = new ValidationErrors();

        // Act & Assert
        errors.IsReadOnly.Should().BeFalse();
    }

    [Fact]
    public void IList_IsFixedSize_ShouldReturnFalse() {
        // Arrange
        // ReSharper disable once CollectionNeverUpdated.Local
        IList errors = new ValidationErrors();

        // Act & Assert
        errors.IsFixedSize.Should().BeFalse();
    }

    [Fact]
    public void ICollectionOfValidationError_Clear_ShouldRemoveAllErrors() {
        // Arrange
        #pragma warning disable IDE0028
        ICollection<ValidationError> errors = new ValidationErrors();
        errors.Add(new("Error1"));
        #pragma warning restore IDE0028

        // Act
        errors.Clear();

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void IListOfValidationError_RemoveAt_ShouldRemoveErrorAtIndex() {
        // Arrange
        #pragma warning disable IDE0028
        IList<ValidationError> errors = new ValidationErrors();
        errors.Add(new("Error1"));
        errors.Add(new("Error2"));
        #pragma warning restore IDE0028

        // Act
        errors.RemoveAt(0);

        // Assert
        errors.Should().ContainSingle();
        errors[0].Message.Should().Be("Error2");
    }

    [Fact]
    public void IEnumerable_GetEnumerator_ShouldReturnEnumeratorThatIteratesOverAllErrors() {
        // Arrange
        IEnumerable errorsEnumerable = new ValidationErrors(new[] {
            new ValidationError("Error1"),
            new ValidationError("Error2"),
        });
        var expectedErrors = new List<ValidationError> {
           new("Error1"),
           new("Error2"),
        };

        // Act
        var errorsList = new List<ValidationError>();
        // ReSharper disable once LoopCanBeConvertedToQuery
        foreach (ValidationError error in errorsEnumerable) {
            errorsList.Add(error);
        }

        // Assert
        errorsList.Should().BeEquivalentTo(expectedErrors);
    }
}
