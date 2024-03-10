namespace DotNetToolbox.AI.Chats;

public class Instructions {
    public string Goal { get; set; } = string.Empty;
    public Dictionary<string, string> CustomValues { get; } = [];
}
