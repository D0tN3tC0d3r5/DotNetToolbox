namespace DotNetToolbox.Graph.Parser;

public class WorkflowLexerTests {
    [Fact]
    public void Tokenize_EmptyInput_ReturnsNoTokens() {
        const string script = "";
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().HaveCount(2);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.EOL, 1, 1, "0"));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.EOF, 1, 1, "0"));
    }

    [Fact]
    public void Tokenize_ActionOnly_ReturnsCorrectTokens() {
        const string script = "DoSomething";
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().HaveCount(3);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 1, "DoSomething"));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.EOL, 1, 12, "11"));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EOF, 1, 12, "11"));
    }

    [Fact]
    public void Tokenize_ActionWithDescription_ReturnsCorrectTokens() {
        const string script = "DoSomething `This is a description`";
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().HaveCount(4);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 1, "DoSomething"));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.Label, 1, 13, "This is a description"));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EOL, 1, 36, "35"));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.EOF, 1, 36, "35"));
    }

    [Fact]
    public void Tokenize_ActionWithDescriptionWithoutSpaces_ReturnsCorrectTokens() {
        const string script = "DoSomething`This is a description`";
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().HaveCount(4);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 1, "DoSomething"));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.Label, 1, 12, "This is a description"));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EOL, 1, 35, "34"));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.EOF, 1, 35, "34"));
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
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EOL, 1, 21, "20"));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.EOF, 1, 21, "20"));
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
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().HaveCount(18);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.If, 1, 1, "IF"));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 4, "Condition"));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EOL, 1, 13, "12"));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.Indent, 2, 1, "1"));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.Then, 2, 3, "THEN"));
        tokens[5].Should().BeEquivalentTo(new Token(TokenType.EOL, 2, 7, "6"));
        tokens[6].Should().BeEquivalentTo(new Token(TokenType.Indent, 3, 1, "2"));
        tokens[7].Should().BeEquivalentTo(new Token(TokenType.Identifier, 3, 5, "Action1"));
        tokens[8].Should().BeEquivalentTo(new Token(TokenType.EOL, 3, 12, "11"));
        tokens[9].Should().BeEquivalentTo(new Token(TokenType.Dedent, 3, 12, "1"));
        tokens[10].Should().BeEquivalentTo(new Token(TokenType.Indent, 4, 1, "1"));
        tokens[11].Should().BeEquivalentTo(new Token(TokenType.Else, 4, 3, "ELSE"));
        tokens[12].Should().BeEquivalentTo(new Token(TokenType.EOL, 4, 7, "6"));
        tokens[13].Should().BeEquivalentTo(new Token(TokenType.Indent, 5, 1, "2"));
        tokens[14].Should().BeEquivalentTo(new Token(TokenType.Identifier, 5, 5, "Action2"));
        tokens[15].Should().BeEquivalentTo(new Token(TokenType.EOL, 5, 12, "11"));
        tokens[16].Should().BeEquivalentTo(new Token(TokenType.Dedent, 5, 12, "2"));
        tokens[17].Should().BeEquivalentTo(new Token(TokenType.EOF, 5, 12, "46"));
    }

    [Fact]
    public void Tokenize_WhenIsOtherwise_ReturnsCorrectTokens() {
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
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EOL, 1, 10, "9"));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.Indent, 2, 1, "1"));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.Is, 2, 3, "IS"));
        tokens[5].Should().BeEquivalentTo(new Token(TokenType.String, 2, 6, "Option1"));
        tokens[6].Should().BeEquivalentTo(new Token(TokenType.EOL, 2, 15, "14"));
        tokens[7].Should().BeEquivalentTo(new Token(TokenType.Indent, 3, 1, "2"));
        tokens[8].Should().BeEquivalentTo(new Token(TokenType.Identifier, 3, 5, "Action1"));
        tokens[9].Should().BeEquivalentTo(new Token(TokenType.EOL, 3, 12, "11"));
        tokens[10].Should().BeEquivalentTo(new Token(TokenType.Dedent, 3, 12, "1"));
        tokens[11].Should().BeEquivalentTo(new Token(TokenType.Indent, 4, 1, "1"));
        tokens[12].Should().BeEquivalentTo(new Token(TokenType.Otherwise, 4, 3, "OTHERWISE"));
        tokens[13].Should().BeEquivalentTo(new Token(TokenType.EOL, 4, 12, "11"));
        tokens[14].Should().BeEquivalentTo(new Token(TokenType.Indent, 5, 1, "2"));
        tokens[15].Should().BeEquivalentTo(new Token(TokenType.Identifier, 5, 5, "Action2"));
        tokens[16].Should().BeEquivalentTo(new Token(TokenType.EOL, 5, 12, "11"));
        tokens[17].Should().BeEquivalentTo(new Token(TokenType.Dedent, 5, 12, "2"));
        tokens[18].Should().BeEquivalentTo(new Token(TokenType.EOF, 5, 12, "56"));
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
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EOL, 1, 7, "6"));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.JumpTo, 2, 1, "GOTO"));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.Identifier, 2, 6, "Label1"));
        tokens[5].Should().BeEquivalentTo(new Token(TokenType.EOL, 2, 12, "11"));
        tokens[6].Should().BeEquivalentTo(new Token(TokenType.EOF, 2, 12, "17"));
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
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.EOL, 1, 27, "26"));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.Identifier, 2, 1, "Action2"));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.EOL, 2, 33, "32"));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.EOF, 2, 8, "58"));
    }

    [Fact]
    public void Tokenize_EmptyLines_IgnoresEmptyLines() {
        const string script = """
                                  Action1


                                  Action2
                                  """;
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().HaveCount(7);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 1, "Action1"));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.EOL, 1, 8, "7"));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.EOL, 2, 1, "0"));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.EOL, 3, 1, "0"));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.Identifier, 4, 1, "Action2"));
        tokens[5].Should().BeEquivalentTo(new Token(TokenType.EOL, 4, 8, "7"));
        tokens[6].Should().BeEquivalentTo(new Token(TokenType.EOF, 4, 8, "14"));
    }

    [Fact]
    public void Tokenize_MixedIndentation_HandlesIndentationCorrectly() {
        const string script = "Action1\n  Action2\n\t\tAction3\n  Action4\nAction5\n";
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().HaveCount(17);
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 1, "Action1"));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.EOL, 1, 8, "7"));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.Indent, 2, 1, "1"));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.Identifier, 2, 3, "Action2"));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.EOL, 2, 10, "9"));
        tokens[5].Should().BeEquivalentTo(new Token(TokenType.Indent, 3, 1, "2"));
        tokens[6].Should().BeEquivalentTo(new Token(TokenType.Identifier, 3, 3, "Action3"));
        tokens[7].Should().BeEquivalentTo(new Token(TokenType.EOL, 3, 10, "9"));
        tokens[8].Should().BeEquivalentTo(new Token(TokenType.Dedent, 3, 10, "1"));
        tokens[9].Should().BeEquivalentTo(new Token(TokenType.Indent, 4, 1, "1"));
        tokens[10].Should().BeEquivalentTo(new Token(TokenType.Identifier, 4, 3, "Action4"));
        tokens[11].Should().BeEquivalentTo(new Token(TokenType.EOL, 4, 10, "9"));
        tokens[12].Should().BeEquivalentTo(new Token(TokenType.Dedent, 4, 10, "1"));
        tokens[13].Should().BeEquivalentTo(new Token(TokenType.Identifier, 5, 1, "Action5"));
        tokens[14].Should().BeEquivalentTo(new Token(TokenType.EOL, 5, 8, "7"));
        tokens[15].Should().BeEquivalentTo(new Token(TokenType.EOL, 6, 1, "0"));
        tokens[16].Should().BeEquivalentTo(new Token(TokenType.EOF, 6, 1, "41"));
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

        tokens.Should().HaveCount(46);
        // Verify the tokens (you may want to check specific tokens)
        tokens[0].Should().BeEquivalentTo(new Token(TokenType.If, 1, 1, "IF"));
        tokens[1].Should().BeEquivalentTo(new Token(TokenType.Identifier, 1, 4, "Condition1"));
        tokens[2].Should().BeEquivalentTo(new Token(TokenType.Equal, 1, 15, "=="));
        tokens[3].Should().BeEquivalentTo(new Token(TokenType.Boolean, 1, 18, "True"));
        tokens[4].Should().BeEquivalentTo(new Token(TokenType.EOL, 1, 22, "21"));
        tokens[5].Should().BeEquivalentTo(new Token(TokenType.Indent, 2, 1, "1"));
        tokens[6].Should().BeEquivalentTo(new Token(TokenType.Then, 2, 3, "THEN"));
        tokens[7].Should().BeEquivalentTo(new Token(TokenType.EOL, 2, 7, "6"));
        tokens[8].Should().BeEquivalentTo(new Token(TokenType.Indent, 3, 1, "2"));
        tokens[9].Should().BeEquivalentTo(new Token(TokenType.Identifier, 3, 5, "Action1"));
        tokens[10].Should().BeEquivalentTo(new Token(TokenType.EOL, 3, 12, "11"));
        tokens[11].Should().BeEquivalentTo(new Token(TokenType.Indent, 4, 1, "2"));
        tokens[12].Should().BeEquivalentTo(new Token(TokenType.Case, 4, 5, "CASE"));
        tokens[13].Should().BeEquivalentTo(new Token(TokenType.Identifier, 4, 10, "Path"));
        tokens[14].Should().BeEquivalentTo(new Token(TokenType.EOL, 4, 14, "13"));
        tokens[15].Should().BeEquivalentTo(new Token(TokenType.Indent, 5, 1, "3"));
        tokens[16].Should().BeEquivalentTo(new Token(TokenType.Is, 5, 7, "IS"));
        tokens[17].Should().BeEquivalentTo(new Token(TokenType.String, 5, 10, "Option1"));
        tokens[18].Should().BeEquivalentTo(new Token(TokenType.EOL, 5, 19, "18"));
        tokens[19].Should().BeEquivalentTo(new Token(TokenType.Indent, 6, 1, "4"));
        tokens[20].Should().BeEquivalentTo(new Token(TokenType.Identifier, 6, 9, "SubAction1"));
        tokens[21].Should().BeEquivalentTo(new Token(TokenType.EOL, 6, 19, "18"));
        tokens[22].Should().BeEquivalentTo(new Token(TokenType.Dedent, 6, 19, "1"));
        tokens[23].Should().BeEquivalentTo(new Token(TokenType.Indent, 7, 1, "3"));
        tokens[24].Should().BeEquivalentTo(new Token(TokenType.Is, 7, 7, "IS"));
        tokens[25].Should().BeEquivalentTo(new Token(TokenType.String, 7, 10, "Option2"));
        tokens[26].Should().BeEquivalentTo(new Token(TokenType.EOL, 7, 19, "18"));
        tokens[27].Should().BeEquivalentTo(new Token(TokenType.Indent, 8, 1, "4"));
        tokens[28].Should().BeEquivalentTo(new Token(TokenType.Identifier, 8, 9, "SubAction2"));
        tokens[29].Should().BeEquivalentTo(new Token(TokenType.EOL, 8, 19, "18"));
        tokens[30].Should().BeEquivalentTo(new Token(TokenType.Dedent, 8, 19, "1"));
        tokens[31].Should().BeEquivalentTo(new Token(TokenType.Indent, 9, 1, "3"));
        tokens[32].Should().BeEquivalentTo(new Token(TokenType.Otherwise, 9, 7, "OTHERWISE"));
        tokens[33].Should().BeEquivalentTo(new Token(TokenType.EOL, 9, 16, "15"));
        tokens[34].Should().BeEquivalentTo(new Token(TokenType.Indent, 10, 1, "4"));
        tokens[35].Should().BeEquivalentTo(new Token(TokenType.Identifier, 10, 9, "SubAction3"));
        tokens[36].Should().BeEquivalentTo(new Token(TokenType.EOL, 10, 19, "18"));
        tokens[37].Should().BeEquivalentTo(new Token(TokenType.Dedent, 10, 19, "3"));
        tokens[38].Should().BeEquivalentTo(new Token(TokenType.Indent, 11, 1, "1"));
        tokens[39].Should().BeEquivalentTo(new Token(TokenType.Else, 11, 3, "ELSE"));
        tokens[40].Should().BeEquivalentTo(new Token(TokenType.EOL, 11, 7, "6"));
        tokens[41].Should().BeEquivalentTo(new Token(TokenType.Indent, 12, 1, "2"));
        tokens[42].Should().BeEquivalentTo(new Token(TokenType.Identifier, 12, 5, "Action2"));
        tokens[43].Should().BeEquivalentTo(new Token(TokenType.EOL, 12, 12, "11"));
        tokens[44].Should().BeEquivalentTo(new Token(TokenType.Dedent, 12, 12, "2"));
        tokens[45].Should().BeEquivalentTo(new Token(TokenType.EOF, 12, 12, "173"));
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
                              IF Date1 == (2023-05-01 12:00:00)
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
        tokens.Should().Contain(t => t.Type == TokenType.DateTime && t.Value == "2023-05-01 12:00:00");
        tokens.Should().Contain(t => t.Type == TokenType.In && t.Value == "IN");
        tokens.Should().Contain(t => t.Type == TokenType.Array && t.Value == "1, 2, 3, 4, 5");
        tokens.Should().Contain(t => t.Type == TokenType.Within && t.Value == "WITHIN");
        tokens.Should().Contain(t => t.Type == TokenType.Range && t.Value == "[0, 100]");
        tokens.Should().Contain(t => t.Type == TokenType.Range && t.Value == "|0, 100]");
        tokens.Should().Contain(t => t.Type == TokenType.Range && t.Value == "[0, 100|");
        tokens.Should().Contain(t => t.Type == TokenType.Range && t.Value == "|0, 100|");
    }

    [Fact]
    public void Tokenize_LiteralValues_ReturnsCorrectTokens() {
        const string script = """
                              IF BoolValue == TRUE
                                THEN
                                  Action1
                              IF NumberValue == 3.14
                                THEN
                                  Action2
                              IF StringValue == "Hello, World!"
                                THEN
                                  Action3
                              IF DateValue == (2023-05-01 12:00:00)
                                THEN
                                  Action4
                              """;
        var tokens = WorkflowLexer.Tokenize(script).ToList();

        tokens.Should().Contain(t => t.Type == TokenType.Boolean && t.Value == "True");
        tokens.Should().Contain(t => t.Type == TokenType.Number && t.Value == "3.14");
        tokens.Should().Contain(t => t.Type == TokenType.String && t.Value == "Hello, World!");
        tokens.Should().Contain(t => t.Type == TokenType.DateTime && t.Value == "2023-05-01 12:00:00");
    }
}
