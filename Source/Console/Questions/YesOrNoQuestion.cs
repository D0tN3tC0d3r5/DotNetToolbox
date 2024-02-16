namespace DotNetToolbox.ConsoleApplication.Questions;

public class YesOrNoQuestion(IOutput output, IInput input, string text, Action<YesOrNo>? configure = null)
    : Question<bool>(output, input, text) {
    public override Result<bool> Ask() {
        var options = new YesOrNo();
        configure?.Invoke(options);
        Output.WriteLine(Text);
        Output.Write($"{options.Yes.Text} | {options.No.Text} ");
        Output.WritePrompt();
        var answer = Input.ReadLine()?.Trim().ToLower() ?? string.Empty;
        return options.Check(answer);
    }
}
