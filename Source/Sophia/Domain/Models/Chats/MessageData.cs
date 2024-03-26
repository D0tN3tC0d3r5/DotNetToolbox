namespace Sophia.Models.Chats;

public class MessageData {
    public long Id { get; set; } = default!;
    public Guid ChatId { get; set; } = default!;
    public int? AgentNumber { get; set; }
    public int Index { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public string Type { get; set; } = string.Empty;
    [MaxLength(20_000)]
    [Required(AllowEmptyStrings = true)]
    public string Content { get; set; } = string.Empty;

    public Message ToModel()
        => new(Type, Content);
}
