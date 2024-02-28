namespace DotNetToolbox.ConsoleApplication.Questions;

public class MultipleChoicePrompt(string question, IEnvironment environment, Action<MultipleChoiceOptions> configure)
    : QuestionPrompt<uint, MultipleChoiceOptions>(question, environment, configure) {
    protected override void ShowPrompt() {
        foreach (var choice in Choices.AsIndexed()) Output.WriteLine($"  {choice.Index + 1}. {choice.Value.Value}");
        Output.Write("Please select one of the choices above ");
        base.ShowPrompt();
    }
}
