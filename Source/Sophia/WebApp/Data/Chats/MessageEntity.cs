namespace Sophia.WebApp.Data.Chats;

[Table("ChatMessages")]
[EntityTypeConfiguration(typeof(MessageEntity))]
public class MessageEntity
    : IEntityTypeConfiguration<MessageEntity> {
    [Required]
    [MaxLength(36)]
    public string ChatId { get; set; } = default!;
    public int Index { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    [MaxLength(20)]
    public string Type { get; set; } = string.Empty;
    [MaxLength(20_000)]
    [Required(AllowEmptyStrings = true)]
    public string Content { get; set; } = string.Empty;

    public void Configure(EntityTypeBuilder<MessageEntity> builder)
        => builder.HasKey(c => new { c.ChatId, c.Index });

    public MessageData ToDto()
        => new() {
            Index = Index,
            Timestamp = Timestamp,
            Type = Type,
            Content = Content,
        };
}
