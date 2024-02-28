namespace DotNetToolbox.ConsoleApplication.Questions;

public class YesOrNoPrompt(string question, IEnvironment environment, Action<YesOrNoOptions>? configure = null)
    : QuestionPrompt<bool, YesOrNoOptions>(question, environment, configure ?? (_ => { })) {
    protected override void ShowPrompt() {
        Output.Write($"Please select {Choices[0].Text} or {Choices[1].Text} ");
        base.ShowPrompt();
    }
}
