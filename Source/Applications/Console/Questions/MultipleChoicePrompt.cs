namespace DotNetToolbox.ConsoleApplication.Questions;

public class MultipleChoicePrompt<TResult>(string question, ISystemEnvironment environment, IEnumerable<MultipleChoiceOption<TResult>> options)
    : QuestionPrompt<TResult>(question, environment) {
    protected override void ShowPrompt() {
        IsNotNullOrEmpty(options);
        foreach (var option in options.OrderBy(o => o.Index).AsIndexed()) Output.WriteLine(option.Value.Display);
        Output.Write("Please select one of the choices above ");
        base.ShowPrompt();
    }

    public override bool Validate(string input, out TResult result) {
        result = default!;
        var choice = options.FirstOrDefault(o => o.Matches(input));
        if (choice is null) return false;
        result = choice.Value;
        return true;
    }
}
