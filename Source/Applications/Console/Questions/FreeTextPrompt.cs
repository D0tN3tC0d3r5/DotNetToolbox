namespace DotNetToolbox.ConsoleApplication.Questions;

public class FreeTextPrompt(string question, IApplicationEnvironment environment, FreeTextOptions options)
    : QuestionPrompt<string>(question, environment) {
    public override bool Validate(string input, out string result) {
        result = string.Empty;
        var isValid = options.ValidateInput(input);
        if (isValid) result = input;
        return isValid;
    }

    protected override string InvalidAnswerMessage => "The answer is not valid.";
}
