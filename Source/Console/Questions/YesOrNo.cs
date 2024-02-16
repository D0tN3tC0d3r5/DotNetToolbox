namespace DotNetToolbox.ConsoleApplication.Questions;

public class YesOrNo : QuestionOptions {
    public AnswerOption Yes { get; set; } = new("Yes", "Y");
    public AnswerOption No { get; set; } = new("No", "N");

    public Result<bool> Check(string input)
        => YesIsSelected(input)
        || NoIsSelected(input);

    private Result<bool> YesIsSelected(string input)
        => Yes.Matches(input);

    private Result<bool> NoIsSelected(string input)
        => !EnforceAnswers
        || No.Matches(input)
               ? false
               : Invalid(false, $"'{input}' is not a valid option.");
}
