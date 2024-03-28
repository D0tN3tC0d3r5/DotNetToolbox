namespace Sophia.WebApp.Data.Chats;

[Table("ChatAgent")]
[EntityTypeConfiguration(typeof(ChatAgentEntity))]
public class ChatAgentEntity
    : IEntityTypeConfiguration<ChatAgentEntity>,
      IHasAgentMessages {
    [MaxLength(36)]
    [Required(AllowEmptyStrings = false)]
    public Guid ChatId { get; set; }
    public ChatEntity Chat { get; set; } = default!;
    public int Number { get; set; }
    [Required]
    [MaxLength(50)]
    public string Provider { get; set; } = default!;
    [Required]
    public int PersonaId { get; set; }
    public PersonaEntity Persona { get; set; } = default!;
    [Required]
    public AgentOptions Options { get; set; } = new();
    public List<MessageEntity> Messages { get; set; } = [];

    public void Configure(EntityTypeBuilder<ChatAgentEntity> builder) {
        builder.HasKey(c => new { c.ChatId, c.Number });
        builder.HasOne(c => c.Chat)
               .WithMany(c => c.Agents)
               .HasForeignKey(c => c.ChatId);
        builder.HasOne(c => c.Persona)
               .WithMany()
               .HasForeignKey(c => c.PersonaId);
        builder.ComplexProperty(c => c.Options);
        builder.HasMany(c => c.Messages)
               .WithOne(c => c.Agent)
               .HasForeignKey(c => new { c.ChatId, c.Index })
               .IsRequired(false)
               .OnDelete(DeleteBehavior.Restrict);
    }

    public ChatAgentData ToDto()
        => new() {
            Persona = Persona.ToDto(),
            Provider = Provider,
            Options = Options,
            Messages = Messages.ToList(m => m.ToDto()),
        };
}
