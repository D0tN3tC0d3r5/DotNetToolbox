namespace System.Linq.Expressions;

public class ExpressionExtensionsTests {
    [Fact]
    public void Test_ExpressionConversionVisitor_VisitLambda() {
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
    public void Test_ExpressionConversionVisitor_VisitConstant() {
        // Arrange
        var mapper = new TypeMapper<int, string>(_ => "3");
        Expression<Func<int, int>> expression = x => 3;
        Expression<Func<string, string>> expectedExpression = x => "3";

        // Act
        var result = expression.ReplaceExpressionType(mapper);

        // Assert
        result.Should().BeEquivalentTo(expectedExpression);
    }

    //[Fact]
    //public void Test_ExpressionConversionVisitor_VisitMember() {
    //    // Arrange
    //    var mapper = new TypeMapper(typeof(int), typeof(string));
    //    var visitor = new ExpressionConversionVisitor(mapper);

    //    // Act
    //    var result = expression.ReplaceExpressionType(mapper);

    //    // Assert
    //    Assert.Null(result);
    //}

    //[Fact]
    //public void Test_ExpressionConversionVisitor_VisitMethodCall() {
    //    // Arrange
    //    var mapper = new TypeMapper(typeof(int), typeof(string));
    //    var visitor = new ExpressionConversionVisitor(mapper);

    //    // Act
    //    var result = visitor.VisitMethodCall(null);

    //    // Assert
    //    Assert.Null(result);
    //}

    //[Fact]
    //public void Test_ExpressionConversionVisitor_VisitBinary() {
    //    // Arrange
    //    var mapper = new TypeMapper(typeof(int), typeof(string));
    //    var visitor = new ExpressionConversionVisitor(mapper);

    //    // Act
    //    var result = visitor.VisitBinary(null);

    //    // Assert
    //    Assert.Null(result);
    //}

    //[Fact]
    //public void Test_ExpressionConversionVisitor_VisitUnary() {
    //    // Arrange
    //    var mapper = new TypeMapper(typeof(int), typeof(string));
    //    var visitor = new ExpressionConversionVisitor(mapper);

    //    // Act
    //    var result = visitor.VisitUnary(null);

    //    // Assert
    //    Assert.Null(result);
    //}

    //[Fact]
    //public void Test_ExpressionConversionVisitor_VisitConditional() {
    //    // Arrange
    //    var mapper = new TypeMapper(typeof(int), typeof(string));
    //    var visitor = new ExpressionConversionVisitor(mapper);

    //    // Act
    //    var result = visitor.VisitConditional(null);

    //    // Assert
    //    Assert.Null(result);
    //}

    //[Fact]
    //public void Test_ExpressionConversionVisitor_VisitMemberInit() {
    //    // Arrange
    //    var mapper = new TypeMapper(typeof(int), typeof(string));
    //    var visitor = new ExpressionConversionVisitor(mapper);

    //    // Act
    //    var result = visitor.VisitMemberInit(null);

    //    // Assert
    //    Assert.Null(result);
    //}

    //[Fact]
    //public void Test_ExpressionConversionVisitor_VisitNewArray() {
    //    // Arrange
    //    var mapper = new TypeMapper(typeof(int), typeof(string));
    //    var visitor = new ExpressionConversionVisitor(mapper);

    //    // Act
    //    var result = visitor.VisitNewArray(null);

    //    // Assert
    //    Assert.Null(result);
    //}
}
