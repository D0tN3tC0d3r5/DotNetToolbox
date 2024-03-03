namespace DotNetToolbox.ConsoleApplication.Questions;

public class YesOrNoOptions
    : QuestionOptions {
    public Choice<bool> Yes { get; set; } = new(true, "Yes", "Y");
    public Choice<bool> No { get; set; } = new(false, "No", "N");
}
