namespace DotNetToolbox.AI.Common;

public interface IContextSection {
    public string Title { get; }
    public string GetIndentedText(string indent);
}
