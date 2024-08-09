namespace DotNetToolbox.ConsoleApplication.Questions;

public class YesOrNoPrompt(string question, IApplicationEnvironment environment, YesOrNoOptions options)
    : QuestionPrompt<bool>(question, environment) {
    protected override void ShowPrompt() {
        Output.Write($"Please select {options.Yes.Display} or {options.No.Display} ");
        base.ShowPrompt();
    }

    public override bool Validate(string input, out bool result) {
        result = default;
        if (options.Yes.Matches(input)) {
            result = true;
            return true;
        }
        if (!options.No.Matches(input)) return false;
        result = false;
        return true;
    }
}
