namespace Sophia.WebApp.Data.Chats;

[Table("Chats")]
[EntityTypeConfiguration(typeof(ChatEntity))]
public class ChatEntity
    : IEntityTypeConfiguration<ChatEntity>
    , IHasMessages {
    [Key]
    [MaxLength(36)]
    [Required(AllowEmptyStrings = false)]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string Id { get; set; } = default!;

    [MaxLength(100)]
    [Required(AllowEmptyStrings = false)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public int PersonaId { get; set; }
    public PersonaEntity Persona { get; set; } = default!;

    public bool IsActive { get; set; }

    [Required]
    [MaxLength(100)]
    public string Model { get; set; } = string.Empty;

    [Required]
    public double Temperature { get; set; }

    public List<MessageEntity> Messages { get; set; } = [];

    public void Configure(EntityTypeBuilder<ChatEntity> builder) {
        builder.HasOne(c => c.Persona)
               .WithMany()
               .HasForeignKey(c => c.PersonaId);
        builder.HasMany(c => c.Messages)
               .WithOne()
               .HasForeignKey(c => c.ChatId);
    }

    public ChatData ToDto()
        => new() {
            Id = Id,
            Title = Title,
            IsActive = IsActive,
            Agent = new() {
                Persona = Persona.ToDto(),
                Model = Model,
                Temperature = Temperature,
            },
            Messages = Messages.ToList(m => m.ToDto()),
        };
}
