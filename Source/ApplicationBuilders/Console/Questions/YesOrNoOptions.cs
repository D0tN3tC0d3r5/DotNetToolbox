namespace DotNetToolbox.ConsoleApplication.Questions;

public class YesOrNoOptions
    : QuestionOptions {
    public MultipleChoiceOption<bool> Yes { get; set; } = new(0, true, "Yes", "Y");
    public MultipleChoiceOption<bool> No { get; set; } = new(1, false, "No", "N");
}
