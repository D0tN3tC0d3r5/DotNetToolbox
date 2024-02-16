namespace DotNetToolbox.ConsoleApplication.Questions;

public class FreeTextQuestion(IOutput output, IInput input, string text, Action<FreeText>? configure = null)
    : Question<string>(output, input, text) {
    public override Result<string> Ask() {
        var freeText = new FreeText();
        configure?.Invoke(freeText);
        Output.WriteLine(Text);
        Output.WritePrompt();
        if (freeText.UseCtrlEnterToSubmit)
            return Success(CaptureInputWithCtrlEnter(Output, Input));

        var answer = Input.ReadLine()?.Trim();
        return Success(answer ?? string.Empty);
    }

    private static string CaptureInputWithCtrlEnter(IOutput output, IInput input) {
        var result = new StringBuilder();
        var keyInfo = input.ReadKey(intercept: true);
        while (IsCtrlEnter(keyInfo)) {
            if (IsEnter(keyInfo)) AddLineBreak(output, result);
            else AddCharacter(keyInfo, output, result);
            keyInfo = input.ReadKey(intercept: true);
        }

        return result.ToString();
    }

    private static bool IsEnter(ConsoleKeyInfo keyInfo) => keyInfo.Key == ConsoleKey.Enter;

    private static bool IsCtrlEnter(ConsoleKeyInfo keyInfo) => keyInfo.Key != ConsoleKey.Enter || (keyInfo.Modifiers & ConsoleModifiers.Control) == 0;

    private static void AddCharacter(ConsoleKeyInfo keyInfo, IOutput output, StringBuilder inputBuilder) {
        output.Write(keyInfo.KeyChar);
        inputBuilder.Append(keyInfo.KeyChar);
    }

    private static void AddLineBreak(IOutput output, StringBuilder inputBuilder) {
        output.WriteLine();
        inputBuilder.AppendLine();
    }
}
