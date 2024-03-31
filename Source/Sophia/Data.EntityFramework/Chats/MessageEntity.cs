namespace Sophia.Data.Chats;

[Table("Messages")]
[EntityTypeConfiguration(typeof(MessageEntity))]
public class MessageEntity
    : IEntityTypeConfiguration<MessageEntity> {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }
    public Guid ChatId { get; set; }
    public int? AgentNumber { get; set; }
    public int Index { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    [Required(AllowEmptyStrings = true)]
    [MaxLength(20)]
    public string Type { get; set; } = string.Empty;
    [MaxLength(20_000)]
    [Required(AllowEmptyStrings = true)]
    public string Content { get; set; } = string.Empty;

    public void Configure(EntityTypeBuilder<MessageEntity> builder)
        => builder.Property(c => c.AgentNumber).IsRequired(false);
}
