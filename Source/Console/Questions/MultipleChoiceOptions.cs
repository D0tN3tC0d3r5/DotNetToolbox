namespace DotNetToolbox.ConsoleApplication.Questions;

public class MultipleChoiceOptions(QuestionPrompt prompt)
    : QuestionOptions<uint>(prompt) {
    public MultipleChoiceOptions AddChoice(string value, string? text = null) {
        Prompt.Choices.Add(new(value, text));
        return this;
    }
    public override bool Validate(string input, out uint result) {
        result = default;
        if (uint.TryParse(input, out var selectedChoice) && selectedChoice >= 1 && selectedChoice <= Prompt.Choices.Count) {
            result = selectedChoice - 1;
            return true;
        }
        var choice = Prompt.Choices.AsIndexed().FirstOrDefault(o => o.Value.Matches(input));
        if (choice is null) return false;
        result = choice.Index;
        return true;
    }
}
