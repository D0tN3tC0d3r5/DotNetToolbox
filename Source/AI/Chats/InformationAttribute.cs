namespace DotNetToolbox.AI.Chats;

public class InformationAttribute(string textFormat, string? nullFormat = null)
    : Attribute {
    public string TextFormat { get; } = textFormat;
    public string? NullFormat { get; } = nullFormat;
}