namespace DotNetToolbox.AI.Chats;

public class Example {
    public required string Title { get; set; }

    public required string Text { get; init; }

    public override string ToString() => $"{Title}: {Text}";
}
