namespace Sophia.Data.Chats;

[Table("Chats")]
[EntityTypeConfiguration(typeof(ChatEntity))]
public class ChatEntity
    : IEntityTypeConfiguration<ChatEntity>
    , IHasChatMessageEntities {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [MaxLength(100)]
    [Required(AllowEmptyStrings = false)]
    public string Title { get; set; } = string.Empty;
    public bool IsActive { get; set; }

    public List<ChatAgentEntity> Agents { get; set; } = [];
    public InstructionsEntity Instructions { get; set; } = new();
    public List<MessageEntity> Messages { get; set; } = [];

    public void Configure(EntityTypeBuilder<ChatEntity> builder) {
        builder.HasMany(c => c.Agents)
               .WithOne(a => a.Chat)
               .HasForeignKey(c => c.ChatId)
               .OnDelete(DeleteBehavior.Cascade);
        builder.ComplexProperty(c => c.Instructions);
        builder.HasMany(c => c.Messages)
               .WithOne(m => m.Chat)
               .HasForeignKey(c => c.ChatId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
