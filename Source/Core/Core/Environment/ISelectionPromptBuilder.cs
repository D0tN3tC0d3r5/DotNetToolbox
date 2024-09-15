namespace DotNetToolbox.Environment;

public interface ISelectionPromptBuilder<TValue>
    where TValue : notnull {
    SelectionPromptBuilder<TValue> WithDefault(TValue defaultValue);
    SelectionPromptBuilder<TValue> ConvertWith(Func<TValue, string> converter);
    SelectionPromptBuilder<TValue> AddChoices(IEnumerable<TValue> choices);
    SelectionPromptBuilder<TValue> AddChoices(TValue choice, params TValue[] otherChoices);
    SelectionPromptBuilder<TValue> ShowResult();
    TValue Show();
    Task<TValue> ShowAsync(CancellationToken ct = default);
}
