namespace DotNetToolbox.ConsoleApplication.Questions;

public class FreeTextOptions
    : QuestionOptions {
    public bool UseCtrlEnterToSubmit { get; set; }
    public Func<string, bool> ValidateInput { get; set; } = _ => true;
}
