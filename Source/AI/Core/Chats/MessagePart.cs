namespace DotNetToolbox.AI.Chats;

public class MessagePart(string type, object value) {
    public string Type { get; set; } = type;
    public object Value { get; set; } = value;

    public string AsText()
        => Type == "text"
               ? (string)Value
               : $"[{Type}]";
}
