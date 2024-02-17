namespace DotNetToolbox.ConsoleApplication.Questions;

public class FreeTextQuestion(IOutput output, IInput input, string text, Action<FreeText>? configure = null)
    : Question<string>(output, input, text) {
    public override Result<string> Ask() {
        var freeText = new FreeText();
        configure?.Invoke(freeText);
        Output.WriteLine(Text);
        Output.WritePrompt();
        if (freeText.UseCtrlEnterToSubmit)
            return Success(Input.ReadMultiLine(Enter, Control));

        var answer = Input.ReadLine()?.Trim();
        return Success(answer ?? string.Empty);
    }
}
