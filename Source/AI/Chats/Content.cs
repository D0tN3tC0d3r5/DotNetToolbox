namespace DotNetToolbox.AI.Chats;

public class Content(string type, object value) {
    public string Type { get; set; } = type;
    public object Value { get; set; } = value;
}
