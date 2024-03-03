namespace DotNetToolbox.ConsoleApplication.Questions;

public class MultipleChoicePrompt(string question, IEnvironment environment, MultipleChoiceOptions options)
    : MultipleChoicePrompt<uint>(question, environment, options);

public class MultipleChoicePrompt<TResult>(string question, IEnvironment environment, MultipleChoiceOptions<TResult> options)
    : QuestionPrompt<TResult>(question, environment) {
    protected override void ShowPrompt() {
        IsNotEmpty(options.Choices);
        foreach (var choice in options.Choices.AsIndexed()) Output.WriteLine(choice.Value.Display);
        Output.Write("Please select one of the choices above ");
        base.ShowPrompt();
    }

    public override bool Validate(string input, out TResult result) {
        result = default!;
        var choice = options.Choices.FirstOrDefault(o => o.Matches(input));
        if (choice is null) return false;
        result = choice.Value;
        return true;
    }
}
