namespace DotNetToolbox.Graph.Parser;

public class WorkflowLexerTests {
    [Fact]
    public void Tokenize_EmptyInput_ReturnsNoTokens() {
        const string script = "";
        var lexer = new WorkflowLexer(script);
        var tokens = lexer.Tokenize().ToList();

        tokens.Should().BeEmpty();
    }

    [Fact]
    public void Tokenize_SingleAction_ReturnsCorrectTokens() {
        const string script = "DoSomething";
        var lexer = new WorkflowLexer(script);
        var tokens = lexer.Tokenize().ToList();

        tokens.Should().HaveCount(2);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Identifier, "DoSomething", 1, 1));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.EOL, "\n", 1, 12));
    }

    [Fact]
    public void Tokenize_ActionWithDescription_ReturnsCorrectTokens() {
        const string script = "DoSomething [This is a description]";
        var lexer = new WorkflowLexer(script);
        var tokens = lexer.Tokenize().ToList();

        tokens.Should().HaveCount(3);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Identifier, "DoSomething", 1, 1));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.Description, "This is a description", 1, 12));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EOL, "\n", 1, 35));
    }

    [Fact]
    public void Tokenize_ActionWithDescriptionWithoutSpaces_ReturnsCorrectTokens() {
        const string script = "DoSomething[This is a description]";
        var lexer = new WorkflowLexer(script);
        var tokens = lexer.Tokenize().ToList();

        tokens.Should().HaveCount(3);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Identifier, "DoSomething", 1, 1));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.Description, "This is a description", 1, 12));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EOL, "\n", 1, 35));
    }

    [Fact]
    public void Tokenize_LabelAndAction_ReturnsCorrectTokens() {
        const string script = """
                                  :Label1
                                  DoSomething
                                  """;
        var lexer = new WorkflowLexer(script);
        var tokens = lexer.Tokenize().ToList();

        tokens.Should().HaveCount(4);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Label, "Label1", 1, 1));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.EOL, "\n", 1, 8));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.Identifier, "DoSomething", 2, 1));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.EOL, "\n", 2, 12));
    }

    [Fact]
    public void Tokenize_IfThenElse_ReturnsCorrectTokens() {
        const string script = """
                              IF Condition
                                THEN
                                  Action1
                                ELSE
                                  Action2
                              """;
        var lexer = new WorkflowLexer(script);
        var tokens = lexer.Tokenize().ToList();

        tokens.Should().HaveCount(15);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.If, "IF", 1, 1));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.Identifier, "Condition", 1, 3));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EOL, "\n", 1, 12));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.Indent, 1, 2, 1));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.Then, "THEN", 2, 3));
        tokens[5].Should().BeEquivalentTo(new Token(TokenType.EOL, "\n", 2, 7));
        tokens[6].Should().BeEquivalentTo(new Token(TokenType.Indent, 2, 3, 1));
        tokens[7].Should().BeEquivalentTo(new Token(TokenType.Identifier, "Action1", 3, 5));
        tokens[8].Should().BeEquivalentTo(new Token(TokenType.EOL, "\n", 3, 12));
        tokens[9].Should().BeEquivalentTo(new Token(TokenType.Indent, 1, 4, 1));
        tokens[10].Should().BeEquivalentTo(new Token(TokenType.Else, "ELSE", 4, 3));
        tokens[11].Should().BeEquivalentTo(new Token(TokenType.EOL, "\n", 4, 7));
        tokens[12].Should().BeEquivalentTo(new Token(TokenType.Indent, 2, 5, 1));
        tokens[13].Should().BeEquivalentTo(new Token(TokenType.Identifier, "Action2", 5, 5));
        tokens[14].Should().BeEquivalentTo(new Token(TokenType.EOL, "\n", 5, 12));
    }

    [Fact]
    public void Tokenize_WhenIsOtherwise_ReturnsCorrectTokens() {
        const string script = """
                              WHEN Path
                                IS "Option1"
                                  Action1
                                OTHERWISE
                                  Action2
                              """;
        var lexer = new WorkflowLexer(script);
        var tokens = lexer.Tokenize().ToList();

        tokens.Should().HaveCount(16);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.When, "WHEN", 1, 1));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.Identifier, "Path", 1, 5));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EOL, "\n", 1, 9));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.Indent, 1, 2, 1));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.Is, "IS", 2, 3));
        tokens[5].Should().BeEquivalentTo(new Token(TokenType.String, "Option1", 2, 5));
        tokens[6].Should().BeEquivalentTo(new Token(TokenType.EOL, "\n", 2, 14));
        tokens[7].Should().BeEquivalentTo(new Token(TokenType.Indent, 2, 3, 1));
        tokens[8].Should().BeEquivalentTo(new Token(TokenType.Identifier, "Action1", 3, 5));
        tokens[9].Should().BeEquivalentTo(new Token(TokenType.EOL, "\n", 3, 12));
        tokens[10].Should().BeEquivalentTo(new Token(TokenType.Indent, 1, 4, 1));
        tokens[11].Should().BeEquivalentTo(new Token(TokenType.Otherwise, "OTHERWISE", 4, 3));
        tokens[12].Should().BeEquivalentTo(new Token(TokenType.EOL, "\n", 4, 12));
        tokens[13].Should().BeEquivalentTo(new Token(TokenType.Indent, 2, 5, 1));
        tokens[14].Should().BeEquivalentTo(new Token(TokenType.Identifier, "Action2", 5, 5));
        tokens[15].Should().BeEquivalentTo(new Token(TokenType.EOL, "\n", 5, 12));
    }

    [Fact]
    public void Tokenize_ExitAndGoto_ReturnsCorrectTokens() {
        const string script = """
                              EXIT 1
                              GOTO Label1
                              """;
        var lexer = new WorkflowLexer(script);
        var tokens = lexer.Tokenize().ToList();

        tokens.Should().HaveCount(6);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Exit, "EXIT", 1, 1));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.Number, "1", 1, 5));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EOL, "\n", 1, 6));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.Goto, "GOTO", 2, 1));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.Identifier, "Label1", 2, 5));
        tokens[5].Should().BeEquivalentTo(new Token(TokenType.EOL, "\n", 2, 11));
    }

    [Fact]
    public void Tokenize_CommentAndMultipleSpaces_IgnoresCommentAndExtraSpaces() {
        const string script = """
                              Action1
                              Action2  # This is a comment
                              """;
        var lexer = new WorkflowLexer(script);
        var tokens = lexer.Tokenize().ToList();

        tokens.Should().HaveCount(4);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Identifier, "Action1", 1, 1));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.EOL, "\n", 1, 8));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.Identifier, "Action2", 2, 1));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.EOL, "\n", 2, 8));
    }

    [Fact]
    public void Tokenize_EmptyLines_IgnoresEmptyLines() {
        const string script = """
                                  Action1


                                  Action2
                                  """;
        var lexer = new WorkflowLexer(script);
        var tokens = lexer.Tokenize().ToList();

        tokens.Should().HaveCount(4);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Identifier, "Action1", 1, 1));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.EOL, "\n", 1, 8));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.Identifier, "Action2", 4, 1));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.EOL, "\n", 4, 8));
    }

    [Fact]
    public void Tokenize_MixedIndentation_HandlesIndentationCorrectly() {
        const string script = "Action1\n  Action2\n\t\tAction3\n  Action4\nAction5\n";
        var lexer = new WorkflowLexer(script);
        var tokens = lexer.Tokenize().ToList();

        tokens.Should().HaveCount(13);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Identifier, "Action1", 1, 1));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.EOL, "\n", 1, 8));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.Indent, 1, 2, 1));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.Identifier, "Action2", 2, 3));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.EOL, "\n", 2, 10));
        tokens[5].Should().BeEquivalentTo(new Token(TokenType.Indent, 2, 3, 1));
        tokens[6].Should().BeEquivalentTo(new Token(TokenType.Identifier, "Action3", 3, 3));
        tokens[7].Should().BeEquivalentTo(new Token(TokenType.EOL, "\n", 3, 10));
        tokens[8].Should().BeEquivalentTo(new Token(TokenType.Indent, 1, 4, 1));
        tokens[9].Should().BeEquivalentTo(new Token(TokenType.Identifier, "Action4", 4, 3));
        tokens[10].Should().BeEquivalentTo(new Token(TokenType.EOL, "\n", 4, 10));
        tokens[11].Should().BeEquivalentTo(new Token(TokenType.Identifier, "Action5", 5, 1));
        tokens[12].Should().BeEquivalentTo(new Token(TokenType.EOL, "\n", 5, 8));
    }
}
