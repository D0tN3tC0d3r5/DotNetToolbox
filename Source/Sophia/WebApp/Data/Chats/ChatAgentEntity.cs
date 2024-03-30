namespace Sophia.WebApp.Data.Chats;

[Table("ChatAgent")]
[EntityTypeConfiguration(typeof(ChatAgentEntity))]
public class ChatAgentEntity
    : IEntityTypeConfiguration<ChatAgentEntity>,
      IHasAgentMessages {
    public Guid ChatId { get; set; }
    public ChatEntity Chat { get; set; } = default!;
    public int Number { get; set; }
    [Required]
    public int PersonaId { get; set; }
    public PersonaEntity Persona { get; set; } = default!;
    public List<MessageEntity> Messages { get; set; } = [];
    [Required]
    public ChatAgentOptionsEntity Options { get; set; } = default!;

    public void Configure(EntityTypeBuilder<ChatAgentEntity> builder) {
        builder.HasKey(c => new { c.ChatId, c.Number });
        builder.HasOne(c => c.Chat)
               .WithMany(c => c.Agents)
               .HasForeignKey(c => c.ChatId);
        builder.HasOne(c => c.Persona)
               .WithMany()
               .HasForeignKey(c => c.PersonaId);
        builder.HasMany(c => c.Messages)
               .WithOne(c => c.Agent)
               .HasForeignKey(c => new { c.ChatId, c.Index })
               .IsRequired(false)
               .OnDelete(DeleteBehavior.Restrict);
        builder.ComplexProperty(c => c.Options);
    }

    public ChatAgentData ToDto(ModelData[] models)
        => new() {
            Persona = Persona.ToDto(),
            Options = Options.ToDto(models),
            Messages = Messages.ToList(m => m.ToDto()),
        };
}
