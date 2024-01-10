namespace DotNetToolbox.ValidationBuilder;

public class StringValidatorsTests {
    public record TestObject : IValidatable {
        private readonly IValidatable _fakePolicy = Substitute.For<IValidatable>();

        public TestObject() {
            _fakePolicy.Validate(Arg.Any<Dictionary<string, object?>>()).Returns(x => {
                var result = Success();
                if (x[0] is not "Invalid") return result;

                result += new ValidationError("Password", "Some error.");
                result += new ValidationError("Password", "Some other error.");
                return result;
            });
        }

        public string? Name { get; init; }
        public string? Email { get; init; }
        public string? Password { get; init; }
        public string Empty { get; } = string.Empty;

        public Result Validate(IDictionary<string, object?>? context = null) {
            var result = Success();
            result += Name.IsRequired()
                          .And().IsNotEmptyOrWhiteSpace()
                          .And().LengthIsAtLeast(3)
                          .And().Contains("ext")
                          .And().LengthIsAtMost(10)
                          .And().LengthIs(5)
                          .And().IsIn("Text1", "Text2", "Text3").Result;
            result += Email.IsRequired()
                           .And().IsNotEmpty()
                           .And().IsEmail().Result;
            result += Password.IsOptional()
                           .And().IsNotEmpty()
                           .And().IsValidPassword(_fakePolicy).Result;
            result += Empty.IsRequired().And().IsEmpty().And().IsEmptyOrWhiteSpace().Result;
            return result;
        }
    }

    private class TestData : TheoryData<TestObject, int> {
        public TestData() {
            Add(new() { Name = "Text1", Email = "some@email.com" }, 0);
            Add(new() { Name = "Text1", Email = "" }, 2);
            Add(new() { Name = "Text1", Email = "NotEmail" }, 1);
            Add(new() { Name = null, Password = "AnyTh1n6!" }, 2);
            Add(new() { Name = "" }, 6);
            Add(new() { Name = "  " }, 6);
            Add(new() { Name = "12" }, 5);
            Add(new() { Name = "12345678901" }, 5);
            Add(new() { Name = "Other", Password = "Invalid" }, 6);
        }
    }

    [Theory]
    [ClassData(typeof(TestData))]
    public void Validate_Validates(TestObject subject, int errorCount) {
        // Act
        var result = subject.Validate();

        // Assert
        result.Errors.Should().HaveCount(errorCount);
    }
}
