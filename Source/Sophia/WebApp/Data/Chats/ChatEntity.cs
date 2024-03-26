namespace Sophia.WebApp.Data.Chats;

[Table("Chats")]
[EntityTypeConfiguration(typeof(ChatEntity))]
public class ChatEntity
    : IEntityTypeConfiguration<ChatEntity>
    , IHasChatMessages {
    [Key]
    [MaxLength(36)]
    [Required(AllowEmptyStrings = false)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; } = default!;
    [MaxLength(100)]
    [Required(AllowEmptyStrings = false)]
    public string Title { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    public List<ChatAgentEntity> Agents { get; set; } = [];
    public List<MessageEntity> Messages { get; set; } = [];

    public void Configure(EntityTypeBuilder<ChatEntity> builder) {
        builder.OwnsMany(c => c.Agents);
        builder.HasMany(c => c.Messages)
               .WithOne()
               .HasForeignKey(c => c.ChatId);
    }

    public ChatData ToDto()
        => new() {
            Id = Id,
            Title = Title,
            IsActive = IsActive,
            Agents = Agents.ToList(a => a.ToDto()),
            Messages = Messages.ToList(m => m.ToDto()),
        };
}
