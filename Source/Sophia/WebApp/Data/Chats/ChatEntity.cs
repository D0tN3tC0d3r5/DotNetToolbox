namespace Sophia.WebApp.Data.Chats;

[Table("Chats")]
[EntityTypeConfiguration(typeof(ChatEntity))]
public class ChatEntity
    : IEntityTypeConfiguration<ChatEntity> {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public int PersonaId { get; set; }
    public PersonaEntity Persona { get; set; } = default!;

    [Required]
    [MaxLength(100)]
    public string Model { get; set; } = string.Empty;

    [Required]
    public double Temperature { get; set; }

    public List<string> Messages { get; set; } = [];

    public void Configure(EntityTypeBuilder<ChatEntity> builder) {
        builder.PrimitiveCollection(c => c.Messages);
        builder.HasOne(c => c.Persona)
               .WithMany()
               .HasForeignKey(c => c.PersonaId);
    }

    public ChatData ToDto()
        => new() {
            Id = Id,
            Agent = new() {
                Persona = Persona.ToDto(),
                Model = Model,
                Temperature = Temperature,
            },
            Messages = Messages.Select(m => new MessageData() {
                IsUserMessage = m.StartsWith("u:"),
                Content = m[2..],
            }).ToList(),
        };
}
