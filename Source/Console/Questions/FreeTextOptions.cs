namespace DotNetToolbox.ConsoleApplication.Questions;

public class FreeTextOptions(QuestionPrompt prompt)
    : QuestionOptions<string>(prompt) {
    public bool UseCtrlEnterToSubmit { get; set; } = false;
    public Func<string, bool> ValidateInput { get; set; } = _ => true;
    public override bool Validate(string input, out string result) {
        result = string.Empty;
        var isValid = ValidateInput(input);
        if (isValid) result = input;
        return isValid;
    }
}
