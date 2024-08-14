namespace DotNetToolbox.Graph.Parser;

public class WorkflowLexerTests {
    [Fact]
    public void Tokenize_EmptyInput_ReturnsNoTokens() {
        const string script = "";
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().HaveCount(1);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.EOF, 0, 0));
    }

    [Fact]
    public void Tokenize_ActionOnly_ReturnsCorrectTokens() {
        const string script = "DoSomething";
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().HaveCount(3);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 1, "DoSomething"));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.EOL, 1, 11));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EOF, 1, 11));
    }

    [Fact]
    public void Tokenize_ActionWithDescription_ReturnsCorrectTokens() {
        const string script = "DoSomething `This is a label`";
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().HaveCount(4);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 1, "DoSomething"));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.Label, 1, 13, "This is a label"));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EOL, 1, 29));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.EOF, 1, 29));
    }

    [Fact]
    public void Tokenize_ActionWithDescriptionWithoutSpaces_ReturnsCorrectTokens() {
        const string script = "DoSomething`This is a label`";
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().HaveCount(4);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 1, "DoSomething"));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.Label, 1, 12, "This is a label"));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EOL, 1, 28));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.EOF, 1, 28));
    }

    [Fact]
    public void Tokenize_LabelAndAction_ReturnsCorrectTokens() {
        const string script = """
                              DoSomething :Label1:
                              """;
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().HaveCount(4);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 1, "DoSomething"));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.Tag, 1, 13, "Label1"));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EOL, 1, 20));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.EOF, 1, 20));
    }

    [Fact]
    public void Tokenize_IfThenElse_ReturnsCorrectTokens() {
        const string script = """
                              IF Condition THEN
                                Action1
                                Action2
                              ELSE
                                Action3
                              Action4
                              """;
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().HaveCount(18);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.If, 1, 1, "IF"));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 4, "Condition"));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.Then, 1, 14, "THEN"));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.EOL, 1, 17));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.Indent, 2));
        tokens[5].Should().BeEquivalentTo(new Token(TokenType.Identifier, 2, 3, "Action1"));
        tokens[6].Should().BeEquivalentTo(new Token(TokenType.EOL, 2, 9));
        tokens[7].Should().BeEquivalentTo(new Token(TokenType.Indent, 3));
        tokens[8].Should().BeEquivalentTo(new Token(TokenType.Identifier, 3, 3, "Action2"));
        tokens[9].Should().BeEquivalentTo(new Token(TokenType.EOL, 3, 9));
        tokens[10].Should().BeEquivalentTo(new Token(TokenType.Else, 4, 1, "ELSE"));
        tokens[11].Should().BeEquivalentTo(new Token(TokenType.EOL, 4, 4));
        tokens[12].Should().BeEquivalentTo(new Token(TokenType.Indent, 5));
        tokens[13].Should().BeEquivalentTo(new Token(TokenType.Identifier, 5, 3, "Action3"));
        tokens[14].Should().BeEquivalentTo(new Token(TokenType.EOL, 5, 9));
        tokens[15].Should().BeEquivalentTo(new Token(TokenType.Identifier, 6, 1, "Action4"));
        tokens[16].Should().BeEquivalentTo(new Token(TokenType.EOL, 6, 7));
        tokens[17].Should().BeEquivalentTo(new Token(TokenType.EOF, 6, 55));
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
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.If, 1, 1, "IF"));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 4, "Condition"));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EOL, 1, 12));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.Indent, 2));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.Identifier, 2, 3, "Action1"));
        tokens[5].Should().BeEquivalentTo(new Token(TokenType.EOL, 2, 9));
        tokens[6].Should().BeEquivalentTo(new Token(TokenType.Else, 3, 1, "ELSE"));
        tokens[7].Should().BeEquivalentTo(new Token(TokenType.EOL, 3, 4));
        tokens[8].Should().BeEquivalentTo(new Token(TokenType.Indent, 4));
        tokens[9].Should().BeEquivalentTo(new Token(TokenType.Identifier, 4, 3, "Action2"));
        tokens[10].Should().BeEquivalentTo(new Token(TokenType.EOL, 4, 9));
        tokens[11].Should().BeEquivalentTo(new Token(TokenType.EOF, 4, 34));
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
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Case, 1, 1, "CASE"));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 6, "Path"));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EOL, 1, 9));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.Indent, 2));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.Is, 2, 3, "IS"));
        tokens[5].Should().BeEquivalentTo(new Token(TokenType.String, 2, 6, "Option1"));
        tokens[6].Should().BeEquivalentTo(new Token(TokenType.EOL, 2, 14));
        tokens[7].Should().BeEquivalentTo(new Token(TokenType.Indent, 3));
        tokens[8].Should().BeEquivalentTo(new Token(TokenType.Indent, 3));
        tokens[9].Should().BeEquivalentTo(new Token(TokenType.Identifier, 3, 5, "Action1"));
        tokens[10].Should().BeEquivalentTo(new Token(TokenType.EOL, 3, 11));
        tokens[11].Should().BeEquivalentTo(new Token(TokenType.Indent, 4));
        tokens[12].Should().BeEquivalentTo(new Token(TokenType.Otherwise, 4, 3, "OTHERWISE"));
        tokens[13].Should().BeEquivalentTo(new Token(TokenType.EOL, 4, 11));
        tokens[14].Should().BeEquivalentTo(new Token(TokenType.Indent, 5));
        tokens[15].Should().BeEquivalentTo(new Token(TokenType.Indent, 5));
        tokens[16].Should().BeEquivalentTo(new Token(TokenType.Identifier, 5, 5, "Action2"));
        tokens[17].Should().BeEquivalentTo(new Token(TokenType.EOL, 5, 11));
        tokens[18].Should().BeEquivalentTo(new Token(TokenType.EOF, 5, 56));
    }

    [Fact]
    public void Tokenize_ExitAndGoto_ReturnsCorrectTokens() {
        const string script = """
                              EXIT 1
                              GOTO Label1
                              """;
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().HaveCount(7);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Exit, 1, 1, "EXIT"));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.Number, 1, 6, "1"));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EOL, 1, 6));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.JumpTo, 2, 1, "GOTO"));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.Identifier, 2, 6, "Label1"));
        tokens[5].Should().BeEquivalentTo(new Token(TokenType.EOL, 2, 11));
        tokens[6].Should().BeEquivalentTo(new Token(TokenType.EOF, 2, 17));
    }

    [Fact]
    public void Tokenize_CommentAndMultipleSpaces_IgnoresCommentAndExtraSpaces() {
        const string script = """
                              Action1# This is a comment
                              Action2 # This is also a comment
                              """;
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().HaveCount(5);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 1, "Action1"));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.EOL, 1, 7));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.Identifier, 2, 1, "Action2"));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.EOL, 2, 7));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.EOF, 2, 14));
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
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.EOL, 1, 7));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.Identifier, 2, 1, "Action2"));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.EOL, 2, 7));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.EOF, 2, 14));
    }

    [Fact]
    public void Tokenize_MixedIndentation_HandlesIndentationCorrectly() {
        const string script = "Action1\n  Action2\n\t\tAction3\n  Action4\nAction5\n";
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().HaveCount(15);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 1, "Action1"));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.EOL, 1, 7));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.Indent, 2));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.Identifier, 2, 3, "Action2"));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.EOL, 2, 9));
        tokens[5].Should().BeEquivalentTo(new Token(TokenType.Indent, 3));
        tokens[6].Should().BeEquivalentTo(new Token(TokenType.Indent, 3));
        tokens[7].Should().BeEquivalentTo(new Token(TokenType.Identifier, 3, 3, "Action3"));
        tokens[8].Should().BeEquivalentTo(new Token(TokenType.EOL, 3, 9));
        tokens[9].Should().BeEquivalentTo(new Token(TokenType.Indent, 4));
        tokens[10].Should().BeEquivalentTo(new Token(TokenType.Identifier, 4, 3, "Action4"));
        tokens[11].Should().BeEquivalentTo(new Token(TokenType.EOL, 4, 9));
        tokens[12].Should().BeEquivalentTo(new Token(TokenType.Identifier, 5, 1, "Action5"));
        tokens[13].Should().BeEquivalentTo(new Token(TokenType.EOL, 5, 7));
        tokens[14].Should().BeEquivalentTo(new Token(TokenType.EOF, 5, 41));
    }

    [Fact]
    public void Tokenize_MultiLevelBranching_ReturnsCorrectTokens() {
        const string script = """
                              IF Condition1 == TRUE
                                THEN
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

        tokens.Should().HaveCount(60);
        // Verify the tokens (you may want to check specific tokens)
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.If, 1, 1, "IF"));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 4, "Condition1"));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.Equal, 1, 15, "=="));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.Boolean, 1, 18, "True"));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.EOL, 1, 21));
        tokens[5].Should().BeEquivalentTo(new Token(TokenType.Indent, 2));
        tokens[6].Should().BeEquivalentTo(new Token(TokenType.Then, 2, 3, "THEN"));
        tokens[7].Should().BeEquivalentTo(new Token(TokenType.EOL, 2, 6));
        tokens[8].Should().BeEquivalentTo(new Token(TokenType.Indent, 3));
        tokens[9].Should().BeEquivalentTo(new Token(TokenType.Indent, 3));
        tokens[10].Should().BeEquivalentTo(new Token(TokenType.Identifier, 3, 5, "Action1"));
        tokens[11].Should().BeEquivalentTo(new Token(TokenType.EOL, 3, 11));
        tokens[12].Should().BeEquivalentTo(new Token(TokenType.Indent, 4));
        tokens[13].Should().BeEquivalentTo(new Token(TokenType.Indent, 4));
        tokens[14].Should().BeEquivalentTo(new Token(TokenType.Case, 4, 5, "CASE"));
        tokens[15].Should().BeEquivalentTo(new Token(TokenType.Identifier, 4, 10, "Path"));
        tokens[16].Should().BeEquivalentTo(new Token(TokenType.EOL, 4, 13));
        tokens[17].Should().BeEquivalentTo(new Token(TokenType.Indent, 5));
        tokens[18].Should().BeEquivalentTo(new Token(TokenType.Indent, 5));
        tokens[19].Should().BeEquivalentTo(new Token(TokenType.Indent, 5));
        tokens[20].Should().BeEquivalentTo(new Token(TokenType.Is, 5, 7, "IS"));
        tokens[21].Should().BeEquivalentTo(new Token(TokenType.String, 5, 10, "Option1"));
        tokens[22].Should().BeEquivalentTo(new Token(TokenType.EOL, 5, 18));
        tokens[23].Should().BeEquivalentTo(new Token(TokenType.Indent, 6));
        tokens[24].Should().BeEquivalentTo(new Token(TokenType.Indent, 6));
        tokens[25].Should().BeEquivalentTo(new Token(TokenType.Indent, 6));
        tokens[26].Should().BeEquivalentTo(new Token(TokenType.Indent, 6));
        tokens[27].Should().BeEquivalentTo(new Token(TokenType.Identifier, 6, 9, "SubAction1"));
        tokens[28].Should().BeEquivalentTo(new Token(TokenType.EOL, 6, 18));
        tokens[29].Should().BeEquivalentTo(new Token(TokenType.Indent, 7));
        tokens[30].Should().BeEquivalentTo(new Token(TokenType.Indent, 7));
        tokens[31].Should().BeEquivalentTo(new Token(TokenType.Indent, 7));
        tokens[32].Should().BeEquivalentTo(new Token(TokenType.Is, 7, 7, "IS"));
        tokens[33].Should().BeEquivalentTo(new Token(TokenType.String, 7, 10, "Option2"));
        tokens[34].Should().BeEquivalentTo(new Token(TokenType.EOL, 7, 18));
        tokens[35].Should().BeEquivalentTo(new Token(TokenType.Indent, 8));
        tokens[36].Should().BeEquivalentTo(new Token(TokenType.Indent, 8));
        tokens[37].Should().BeEquivalentTo(new Token(TokenType.Indent, 8));
        tokens[38].Should().BeEquivalentTo(new Token(TokenType.Indent, 8));
        tokens[39].Should().BeEquivalentTo(new Token(TokenType.Identifier, 8, 9, "SubAction2"));
        tokens[40].Should().BeEquivalentTo(new Token(TokenType.EOL, 8, 18));
        tokens[41].Should().BeEquivalentTo(new Token(TokenType.Indent, 9));
        tokens[42].Should().BeEquivalentTo(new Token(TokenType.Indent, 9));
        tokens[43].Should().BeEquivalentTo(new Token(TokenType.Indent, 9));
        tokens[44].Should().BeEquivalentTo(new Token(TokenType.Otherwise, 9, 7, "OTHERWISE"));
        tokens[45].Should().BeEquivalentTo(new Token(TokenType.EOL, 9, 15));
        tokens[46].Should().BeEquivalentTo(new Token(TokenType.Indent, 10));
        tokens[47].Should().BeEquivalentTo(new Token(TokenType.Indent, 10));
        tokens[48].Should().BeEquivalentTo(new Token(TokenType.Indent, 10));
        tokens[49].Should().BeEquivalentTo(new Token(TokenType.Indent, 10));
        tokens[50].Should().BeEquivalentTo(new Token(TokenType.Identifier, 10, 9, "SubAction3"));
        tokens[51].Should().BeEquivalentTo(new Token(TokenType.EOL, 10, 18));
        tokens[52].Should().BeEquivalentTo(new Token(TokenType.Indent, 11));
        tokens[53].Should().BeEquivalentTo(new Token(TokenType.Else, 11, 3, "ELSE"));
        tokens[54].Should().BeEquivalentTo(new Token(TokenType.EOL, 11, 6));
        tokens[55].Should().BeEquivalentTo(new Token(TokenType.Indent, 12));
        tokens[56].Should().BeEquivalentTo(new Token(TokenType.Indent, 12));
        tokens[57].Should().BeEquivalentTo(new Token(TokenType.Identifier, 12, 5, "Action2"));
        tokens[58].Should().BeEquivalentTo(new Token(TokenType.EOL, 12, 11));
        tokens[59].Should().BeEquivalentTo(new Token(TokenType.EOF, 12, 173));
    }

    [Fact]
    public void Tokenize_ComplexPredicates_ReturnsCorrectTokens() {
        const string script = """
                              IF Value1 > 5 AND Value2 < 10
                                THEN
                                  Action1
                              IF Value1 >= 5 OR Value2 <= 10
                                THEN
                                  Action1
                              IF Value1 != 3.14
                                THEN
                                  Action1
                              IF Date1 == '2023-05-01 12:00:00'
                                THEN
                                  Action2
                              IF List1 NOT IN {1, 2, 3, 4, 5}
                                THEN
                                  Action3
                              IF Range1 WITHIN [0, 100]
                                THEN
                                  Action4
                              IF Range1 WITHIN [0, 100|
                                THEN
                                  Action5
                              IF Range1 WITHIN |0, 100]
                                THEN
                                  Action6
                              IF Range1 WITHIN |0, 100|
                                THEN
                                  Action7
                              """;
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().Contain(t => t.Type == TokenType.LessThan && t.Value == "<");
        tokens.Should().Contain(t => t.Type == TokenType.GreaterThan && t.Value == ">");
        tokens.Should().Contain(t => t.Type == TokenType.LessOrEqual && t.Value == "<=");
        tokens.Should().Contain(t => t.Type == TokenType.GreaterOrEqual && t.Value == ">=");
        tokens.Should().Contain(t => t.Type == TokenType.And && t.Value == "AND");
        tokens.Should().Contain(t => t.Type == TokenType.Or && t.Value == "OR");
        tokens.Should().Contain(t => t.Type == TokenType.Not && t.Value == "NOT");
        tokens.Should().Contain(t => t.Type == TokenType.Equal && t.Value == "==");
        tokens.Should().Contain(t => t.Type == TokenType.NotEqual && t.Value == "!=");
        tokens.Should().Contain(t => t.Type == TokenType.DateTime && t.Value == "2023-05-01T12:00:00.0000000");
        tokens.Should().Contain(t => t.Type == TokenType.In && t.Value == "IN");
        tokens.Should().Contain(t => t.Type == TokenType.Array && t.Value == "1, 2, 3, 4, 5");
        tokens.Should().Contain(t => t.Type == TokenType.Within && t.Value == "WITHIN");
        tokens.Should().Contain(t => t.Type == TokenType.Range && t.Value == "[0, 100]");
        tokens.Should().Contain(t => t.Type == TokenType.Range && t.Value == "|0, 100]");
        tokens.Should().Contain(t => t.Type == TokenType.Range && t.Value == "[0, 100|");
        tokens.Should().Contain(t => t.Type == TokenType.Range && t.Value == "|0, 100|");
    }

    [Fact]
    public void Tokenize_DateTime_ReturnsCorrectTokens() {
        const string script = """
                              IF DateValue == '2023-05-01 12:00:00'
                                Action1
                              """;
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        // Verify the tokens (you may want to check specific tokens)
        tokens[3].Value.Should().Be("2023-05-01T12:00:00.0000000");
    }

    [Fact]
    public void Tokenize_LiteralValues_ReturnsCorrectTokens() {
        const string script = """
                              IF BoolValue == TRUE
                                Action1
                              IF NumberValue == 3.14
                                Action2
                              IF StringValue == "Hello, World!"
                                Action3
                              IF DateValue == '2023-05-01'
                                Action4
                              """;
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens[3].Value.Should().Be("True");
        tokens[11].Value.Should().Be("3.14");
        tokens[19].Value.Should().Be("Hello, World!");
        tokens[27].Value.Should().Be("2023-05-01T00:00:00.0000000");
    }
}
