namespace DotNetToolbox.Graph.Parser;

public class WorkflowLexerTests {
    [Fact]
    public void Tokenize_EmptyInput_ReturnsNoTokens() {
        const string script = "";
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().HaveCount(1);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.EndOfFile));
    }

    [Fact]
    public void Tokenize_ActionOnly_ReturnsCorrectTokens() {
        const string script = "DoSomething";
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().HaveCount(3);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 1, "DoSomething"));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 1, 11));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EndOfFile, 1, 11));
    }

    [Fact]
    public void Tokenize_ActionWithDescription_ReturnsCorrectTokens() {
        const string script = "DoSomething `This is a label`";
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().HaveCount(4);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 1, "DoSomething"));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.Label, 1, 13, "This is a label"));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 1, 29));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.EndOfFile, 1, 29));
    }

    [Fact]
    public void Tokenize_ActionWithDescriptionWithoutSpaces_ReturnsCorrectTokens() {
        const string script = "DoSomething`This is a label`";
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().HaveCount(4);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 1, "DoSomething"));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.Label, 1, 12, "This is a label"));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 1, 28));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.EndOfFile, 1, 28));
    }

    [Fact]
    public void Tokenize_LabelAndAction_ReturnsCorrectTokens() {
        const string script = "DoSomething :Label1:";
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().HaveCount(4);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 1, "DoSomething"));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.Tag, 1, 13, "Label1"));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 1, 20));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.EndOfFile, 1, 20));
    }

    [Fact]
    public void Tokenize_IfThenElse_ReturnsCorrectTokens() {
        const string script = """
                              IF Condition
                                Action1
                                Action2
                              ELSE
                                Action3
                              Action4
                              """;
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().HaveCount(17);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.If, 1, 1));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 4, "Condition"));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 1, 12));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.Indent, 2));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.Identifier, 2, 3, "Action1"));
        tokens[5].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 2, 9));
        tokens[6].Should().BeEquivalentTo(new Token(TokenType.Indent, 3));
        tokens[7].Should().BeEquivalentTo(new Token(TokenType.Identifier, 3, 3, "Action2"));
        tokens[8].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 3, 9));
        tokens[9].Should().BeEquivalentTo(new Token(TokenType.Else, 4, 1));
        tokens[10].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 4, 4));
        tokens[11].Should().BeEquivalentTo(new Token(TokenType.Indent, 5));
        tokens[12].Should().BeEquivalentTo(new Token(TokenType.Identifier, 5, 3, "Action3"));
        tokens[13].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 5, 9));
        tokens[14].Should().BeEquivalentTo(new Token(TokenType.Identifier, 6, 1, "Action4"));
        tokens[15].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 6, 7));
        tokens[16].Should().BeEquivalentTo(new Token(TokenType.EndOfFile, 6, 50));
    }

    [Fact]
    public void Tokenize_IfElse_ReturnsCorrectTokens() {
        const string script = """
                              IF Condition
                                Action1
                              ELSE
                                Action2
                              """;
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().HaveCount(12);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.If, 1, 1));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 4, "Condition"));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 1, 12));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.Indent, 2));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.Identifier, 2, 3, "Action1"));
        tokens[5].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 2, 9));
        tokens[6].Should().BeEquivalentTo(new Token(TokenType.Else, 3, 1));
        tokens[7].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 3, 4));
        tokens[8].Should().BeEquivalentTo(new Token(TokenType.Indent, 4));
        tokens[9].Should().BeEquivalentTo(new Token(TokenType.Identifier, 4, 3, "Action2"));
        tokens[10].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 4, 9));
        tokens[11].Should().BeEquivalentTo(new Token(TokenType.EndOfFile, 4, 34));
    }

    [Fact]
    public void Tokenize_CaseIsOtherwise_ReturnsCorrectTokens() {
        const string script = """
                              CASE Path
                                IS "Option1"
                                  Action1
                                OTHERWISE
                                  Action2
                              """;
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().HaveCount(19);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Case, 1, 1));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 6, "Path"));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 1, 9));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.Indent, 2));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.Is, 2, 3));
        tokens[5].Should().BeEquivalentTo(new Token(TokenType.String, 2, 6, "Option1"));
        tokens[6].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 2, 14));
        tokens[7].Should().BeEquivalentTo(new Token(TokenType.Indent, 3));
        tokens[8].Should().BeEquivalentTo(new Token(TokenType.Indent, 3));
        tokens[9].Should().BeEquivalentTo(new Token(TokenType.Identifier, 3, 5, "Action1"));
        tokens[10].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 3, 11));
        tokens[11].Should().BeEquivalentTo(new Token(TokenType.Indent, 4));
        tokens[12].Should().BeEquivalentTo(new Token(TokenType.Otherwise, 4, 3));
        tokens[13].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 4, 11));
        tokens[14].Should().BeEquivalentTo(new Token(TokenType.Indent, 5));
        tokens[15].Should().BeEquivalentTo(new Token(TokenType.Indent, 5));
        tokens[16].Should().BeEquivalentTo(new Token(TokenType.Identifier, 5, 5, "Action2"));
        tokens[17].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 5, 11));
        tokens[18].Should().BeEquivalentTo(new Token(TokenType.EndOfFile, 5, 56));
    }

    [Fact]
    public void Tokenize_ExitAndGoto_ReturnsCorrectTokens() {
        const string script = """
                              EXIT 1
                              GOTO Label1
                              """;
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().HaveCount(7);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Exit, 1, 1));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.Number, 1, 6, "1"));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 1, 6));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.JumpTo, 2, 1));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.Identifier, 2, 6, "Label1"));
        tokens[5].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 2, 11));
        tokens[6].Should().BeEquivalentTo(new Token(TokenType.EndOfFile, 2, 17));
    }

    [Fact]
    public void Tokenize_CommentAndMultipleSpaces_IgnoresCommentAndExtraSpaces() {
        const string script = """
                              # This is a comment
                              Action1# This is another comment
                              Action2 # This is also a comment
                              """;
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().HaveCount(5);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 1, "Action1"));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 1, 7));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.Identifier, 2, 1, "Action2"));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 2, 7));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.EndOfFile, 2, 14));
    }

    [Fact]
    public void Tokenize_EmptyLines_IgnoresEmptyLines() {
        const string script = """
                                  Action1


                                  Action2
                                  """;
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().HaveCount(5);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 1, "Action1"));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 1, 7));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.Identifier, 2, 1, "Action2"));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 2, 7));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.EndOfFile, 2, 14));
    }

    [Fact]
    public void Tokenize_MixedIndentation_HandlesIndentationCorrectly() {
        const string script = "Action1\n  Action2\n\t\tAction3\n  Action4\nAction5\n";
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().HaveCount(15);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 1, "Action1"));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 1, 7));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.Indent, 2));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.Identifier, 2, 3, "Action2"));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 2, 9));
        tokens[5].Should().BeEquivalentTo(new Token(TokenType.Indent, 3));
        tokens[6].Should().BeEquivalentTo(new Token(TokenType.Indent, 3));
        tokens[7].Should().BeEquivalentTo(new Token(TokenType.Identifier, 3, 3, "Action3"));
        tokens[8].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 3, 9));
        tokens[9].Should().BeEquivalentTo(new Token(TokenType.Indent, 4));
        tokens[10].Should().BeEquivalentTo(new Token(TokenType.Identifier, 4, 3, "Action4"));
        tokens[11].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 4, 9));
        tokens[12].Should().BeEquivalentTo(new Token(TokenType.Identifier, 5, 1, "Action5"));
        tokens[13].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 5, 7));
        tokens[14].Should().BeEquivalentTo(new Token(TokenType.EndOfFile, 5, 41));
    }

    [Fact]
    public void Tokenize_MultiLevelBranching_ReturnsCorrectTokens() {
        const string script = """
                              IF Condition1 == TRUE
                                Action1
                                CASE Path
                                  IS "Option1"
                                    SubAction1
                                  IS "Option2"
                                    SubAction2
                                  OTHERWISE
                                    SubAction3
                              ELSE
                                Action2
                              """;
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().HaveCount(47);
        // Verify the tokens (you may want to check specific tokens)
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.If, 1, 1));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 4, "Condition1"));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.Equal, 1, 15));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.Boolean, 1, 18, "True"));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 1, 21));
        tokens[5].Should().BeEquivalentTo(new Token(TokenType.Indent, 2));
        tokens[6].Should().BeEquivalentTo(new Token(TokenType.Identifier, 2, 3, "Action1"));
        tokens[7].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 2, 9));
        tokens[8].Should().BeEquivalentTo(new Token(TokenType.Indent, 3));
        tokens[9].Should().BeEquivalentTo(new Token(TokenType.Case, 3, 3));
        tokens[10].Should().BeEquivalentTo(new Token(TokenType.Identifier, 3, 8, "Path"));
        tokens[11].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 3, 11));
        tokens[12].Should().BeEquivalentTo(new Token(TokenType.Indent, 4));
        tokens[13].Should().BeEquivalentTo(new Token(TokenType.Indent, 4));
        tokens[14].Should().BeEquivalentTo(new Token(TokenType.Is, 4, 5));
        tokens[15].Should().BeEquivalentTo(new Token(TokenType.String, 4, 8, "Option1"));
        tokens[16].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 4, 16));
        tokens[17].Should().BeEquivalentTo(new Token(TokenType.Indent, 5));
        tokens[18].Should().BeEquivalentTo(new Token(TokenType.Indent, 5));
        tokens[19].Should().BeEquivalentTo(new Token(TokenType.Indent, 5));
        tokens[20].Should().BeEquivalentTo(new Token(TokenType.Identifier, 5, 7, "SubAction1"));
        tokens[21].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 5, 16));
        tokens[22].Should().BeEquivalentTo(new Token(TokenType.Indent, 6));
        tokens[23].Should().BeEquivalentTo(new Token(TokenType.Indent, 6));
        tokens[24].Should().BeEquivalentTo(new Token(TokenType.Is, 6, 5));
        tokens[25].Should().BeEquivalentTo(new Token(TokenType.String, 6, 8, "Option2"));
        tokens[26].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 6, 16));
        tokens[27].Should().BeEquivalentTo(new Token(TokenType.Indent, 7));
        tokens[28].Should().BeEquivalentTo(new Token(TokenType.Indent, 7));
        tokens[29].Should().BeEquivalentTo(new Token(TokenType.Indent, 7));
        tokens[30].Should().BeEquivalentTo(new Token(TokenType.Identifier, 7, 7, "SubAction2"));
        tokens[31].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 7, 16));
        tokens[32].Should().BeEquivalentTo(new Token(TokenType.Indent, 8));
        tokens[33].Should().BeEquivalentTo(new Token(TokenType.Indent, 8));
        tokens[34].Should().BeEquivalentTo(new Token(TokenType.Otherwise, 8, 5));
        tokens[35].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 8, 13));
        tokens[36].Should().BeEquivalentTo(new Token(TokenType.Indent, 9));
        tokens[37].Should().BeEquivalentTo(new Token(TokenType.Indent, 9));
        tokens[38].Should().BeEquivalentTo(new Token(TokenType.Indent, 9));
        tokens[39].Should().BeEquivalentTo(new Token(TokenType.Identifier, 9, 7, "SubAction3"));
        tokens[40].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 9, 16));
        tokens[41].Should().BeEquivalentTo(new Token(TokenType.Else, 10, 1));
        tokens[42].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 10, 4));
        tokens[43].Should().BeEquivalentTo(new Token(TokenType.Indent, 11));
        tokens[44].Should().BeEquivalentTo(new Token(TokenType.Identifier, 11, 3, "Action2"));
        tokens[45].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 11, 9));
        tokens[46].Should().BeEquivalentTo(new Token(TokenType.EndOfFile, 11, 147));
    }

    [Fact]
    public void Tokenize_ComplexPredicates_ReturnsCorrectTokens() {
        const string script = """
                              ==
                              !=
                              >
                              >=
                              <
                              <=
                              NOT
                              AND
                              OR
                              IN
                              WITHIN
                              """;
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens[0].Type.Should().Be(TokenType.Equal);
        tokens[2].Type.Should().Be(TokenType.NotEqual);
        tokens[4].Type.Should().Be(TokenType.GreaterThan);
        tokens[6].Type.Should().Be(TokenType.GreaterOrEqual);
        tokens[8].Type.Should().Be(TokenType.LessThan);
        tokens[10].Type.Should().Be(TokenType.LessOrEqual);
        tokens[12].Type.Should().Be(TokenType.Not);
        tokens[14].Type.Should().Be(TokenType.And);
        tokens[16].Type.Should().Be(TokenType.Or);
        tokens[18].Type.Should().Be(TokenType.In);
        tokens[20].Type.Should().Be(TokenType.Within);
    }

    [Fact]
    public void Tokenize_LiteralValues_ReturnsCorrectTokens() {
        const string script = """
                              TRUE
                              FALSE
                              42
                              3.14
                              "Hello, World!"
                              '2023-05-01'
                              { 1, 2, 3, 4, 5 }
                              [0, 100]
                              [0, 100|
                              |0, 100]
                              |0, 100|
                              """;
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens[0].Value.Should().Be("True");
        tokens[2].Value.Should().Be("False");
        tokens[4].Value.Should().Be("42");
        tokens[6].Value.Should().Be("3.14");
        tokens[8].Value.Should().Be("Hello, World!");
        tokens[10].Value.Should().Be("2023-05-01T00:00:00.0000000");
        tokens[12].Value.Should().Be("1,2,3,4,5");
        tokens[14].Value.Should().Be("[0, 100]");
        tokens[16].Value.Should().Be("[0, 100|");
        tokens[18].Value.Should().Be("|0, 100]");
        tokens[20].Value.Should().Be("|0, 100|");
    }

    [Theory]
    [InlineData("1InvalidStart", "Identifier must start with a letter.")]
    [InlineData("TooLongIdentifierTooLongIdentifierTooLongIdentifierTooLongIdentifierTooLong", "Identifier must not exceed 64 characters.")]
    [InlineData("Invalid-Identifier", "Identifier can only contain letters, numbers, and underscores.")]
    [InlineData("Invalid$Identifier", "Identifier can only contain letters, numbers, and underscores.")]
    [InlineData("+", "Invalid token.")]
    public void Tokenize_InvalidIdentifier_ReturnsErrorToken(string invalidIdentifier, string expectedErrorMessage) {
        // Act
        var tokens = WorkflowLexer.Tokenize(invalidIdentifier).ToList();

        // Assert
        tokens.Should().HaveCount(3);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Error, 1, 1, expectedErrorMessage));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 1, invalidIdentifier.Length));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EndOfFile, 1, invalidIdentifier.Length));
    }

    [Theory]
    [InlineData("ValidIdentifier")]
    [InlineData("Valid_Identifier_123")]
    [InlineData("a")] // Minimum length
    [InlineData("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_")] // Maximum length (64 characters)
    public void Tokenize_ValidIdentifier_ReturnsIdentifierToken(string validIdentifier) {
        // Act
        var tokens = WorkflowLexer.Tokenize(validIdentifier).ToList();

        // Assert
        tokens.Should().HaveCount(3);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 1, validIdentifier));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 1, validIdentifier.Length));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EndOfFile, 1, validIdentifier.Length));
    }

    [Fact]
    public void Tokenize_MixOfValidAndInvalidIdentifiers_ReturnsCorrectTokens() {
        // Arrange
        const string script = """
                              ValidIdentifier
                              1InvalidStart
                              Another_Valid_Identifier
                              Invalid-Identifier
                              """;

        // Act
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        // Assert
        tokens.Should().HaveCount(9);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 1, "ValidIdentifier"));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 1, 15));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.Error, 2, 1, "Identifier must start with a letter."));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 2, 13));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.Identifier, 3, 1, "Another_Valid_Identifier"));
        tokens[5].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 3, 24));
        tokens[6].Should().BeEquivalentTo(new Token(TokenType.Error, 4, 1, "Identifier can only contain letters, numbers, and underscores."));
        tokens[7].Should().BeEquivalentTo(new Token(TokenType.EndOfLine, 4, 18));
        tokens[8].Should().BeEquivalentTo(new Token(TokenType.EndOfFile, 4, 70));
    }
}
