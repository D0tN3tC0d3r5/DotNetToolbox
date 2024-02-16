namespace DotNetToolbox.ConsoleApplication.Questions;

public class MultipleChoiceQuestion(IOutput output, IInput input, string text, Action<MultipleChoice> configure)
    : Question<int>(output, input, text) {
    public override Result<int> Ask() {
        var options = new MultipleChoice();
        IsNotNull(configure)(options);
        IsNotEmpty(options.Choices);
        Output.WriteLine(Text);
        foreach (var choice in options.Choices.AsIndexed())
            Output.WriteLine($"  {choice.Index + 1}. {choice.Value.Value}");
        Output.Write("Please select one of the choices above ");
        Output.WritePrompt();

        var answer = Input.ReadLine()?.Trim().ToLower() ?? string.Empty;
        return options.Check(answer);
    }
}
