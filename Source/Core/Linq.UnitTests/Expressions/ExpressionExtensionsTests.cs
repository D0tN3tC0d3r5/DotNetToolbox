namespace System.Linq.Expressions;

public class ExpressionExtensionsTests {
    private sealed record TestResult {
        public required string Output { get; init; }
    }
    private sealed record TestModel(string Name, int Age);
    private sealed record TestEntity(string Name, int Age);

    [Fact]
    public void VisitLambda_ConvertsExpression() {
        // Arrange
        var mapper = new TypeMapper<int, string>(i => $"{i}");
        Expression<Func<int, int>> expression = x => x;
        Expression<Func<string, string>> expectedExpression = x => x;

        // Act
        var result = expression.ReplaceExpressionType(mapper);

        // Assert
        result.Should().BeEquivalentTo(expectedExpression);
    }

    [Fact]
    public void VisitConstant_WithoutMapper_ReturnsExpression() {
        // Arrange
        Expression<Func<int, int>> expression = _ => 3;
        Expression<Func<int, int>> expectedExpression = _ => 3;

        // Act
        var result = expression.ReplaceExpressionType();

        // Assert
        result.Should().BeEquivalentTo(expectedExpression);
    }

    [Fact]
    [Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1861:Avoid constant arrays as arguments", Justification = "Testing expression")]
    public void VisitArray_ConvertsExpression() {
        // Arrange
        var arrayMapper = new TypeMapper<string[], int[]>();
        var elementMapper = new TypeMapper<string, int>(int.Parse);
        Expression<Func<string[], bool>> expression = x => x.SequenceEqual(new[] { "1", "2", "3" });
        Expression<Func<int[], bool>> expectedExpression = x => x.SequenceEqual(new[] { 1, 2, 3 });

        // Act
        var result = expression.ReplaceExpressionType(arrayMapper, elementMapper);

        // Assert
        result.Should().BeEquivalentTo(expectedExpression);
    }

    [Fact]
    public void VisitArray_FromVariable_Throws() {
        // Arrange
        var source = new[] { "1", "2", "3" };
        var arrayMapper = new TypeMapper<string[], int[]>();
        var elementMapper = new TypeMapper<string, int>(int.Parse);
        Expression<Func<string[], bool>> expression = x => x.SequenceEqual(source);

        // Act
        var action = () => expression.ReplaceExpressionType(arrayMapper, elementMapper);

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Visit_ForOperators_ConvertsExpression() {
        // Arrange
        var arrayMapper = new TypeMapper<TestModel[], TestEntity[]>();
        var elementMapper = new TypeMapper<TestModel, TestEntity>(x => new(x.Name, x.Age));
        Expression<Func<TestModel[], int>> expression = x => x.Length == 0 ? 0 : -x.Length + x.Sum(i => i.Age);
        Expression<Func<TestEntity[], int>> expectedExpression = x => x.Length == 0 ? 0 : -x.Length + x.Sum(i => i.Age);

        // Act
        var result = expression.ReplaceExpressionType(arrayMapper, elementMapper);

        // Assert
        result.Should().BeEquivalentTo(expectedExpression);
    }

    [Fact]
    public void Visit_New_ConvertsExpression() {
        // Arrange
        var arrayMapper = new TypeMapper<TestModel[], TestEntity[]>();
        var elementMapper = new TypeMapper<TestModel, TestEntity>(x => new(x.Name, x.Age));
        Expression<Func<TestModel[], IEnumerable<TestModel>>> expression = x => x.Select(i => new TestModel(i.Name, i.Age));
        Expression<Func<TestEntity[], IEnumerable<TestEntity>>> expectedExpression = x => x.Select(i => new TestEntity(i.Name, i.Age));

        // Act
        var result = expression.ReplaceExpressionType(arrayMapper, elementMapper);

        // Assert
        result.Should().BeEquivalentTo(expectedExpression);
    }

    [Fact]
    public void Visit_MemberBinding_ConvertsExpression() {
        // Arrange
        var arrayMapper = new TypeMapper<TestModel[], TestEntity[]>();
        var elementMapper = new TypeMapper<TestModel, TestEntity>(x => new(x.Name, x.Age));
        Expression<Func<TestModel[], IEnumerable<string>>> expression = x => x.Select(i => new TestResult { Output = $"{i.Name}: {i.Age}y" }).Select(o => o.Output.Trim());
        Expression<Func<TestEntity[], IEnumerable<string>>> expectedExpression = x => x.Select(i => new TestResult { Output = $"{i.Name}: {i.Age}y" }).Select(o => o.Output.Trim());

        // Act
        var result = expression.ReplaceExpressionType(arrayMapper, elementMapper);

        // Assert
        result.Should().BeEquivalentTo(expectedExpression);
    }
}
