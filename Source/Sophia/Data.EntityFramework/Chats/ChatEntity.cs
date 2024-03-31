namespace Sophia.Data.Chats;

[Table("Chats")]
[EntityTypeConfiguration(typeof(ChatEntity))]
public class ChatEntity
    : IEntity<Guid>,
      IEntityTypeConfiguration<ChatEntity>
    , IHasChatMessageEntities {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [MaxLength(100)]
    [Required(AllowEmptyStrings = false)]
    public string Title { get; set; } = default!;
    public bool IsActive { get; set; }

    public List<ChatAgentEntity> Agents { get; set; } = [];
    public InstructionsEntity Instructions { get; set; } = new();
    public List<MessageEntity> Messages { get; set; } = [];

    public void Configure(EntityTypeBuilder<ChatEntity> builder) {
        builder.HasMany(c => c.Agents)
               .WithOne()
               .HasForeignKey(c => c.ChatId)
               .OnDelete(DeleteBehavior.ClientCascade);
        builder.ComplexProperty(c => c.Instructions);
        builder.HasMany(c => c.Messages)
               .WithOne()
               .HasForeignKey(c => new { c.ChatId, AgentNumber = (int?)null })
               .OnDelete(DeleteBehavior.ClientCascade);
    }
}
