namespace Sophia.Data.Personas;

[Table("Personas")]
[EntityTypeConfiguration(typeof(PersonaEntity))]
public class PersonaEntity
    : ISimpleKeyEntity<PersonaEntity, int>,
      IEntityTypeConfiguration<PersonaEntity>
    , IHasTools<int>
    , IHasFacts<int> {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = default!;
    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = default!;
    public CharacteristicsEntity Characteristics { get; set; } = new();
    public List<string> Facts { get; set; } = [];
    public List<ToolEntity> Tools { get; set; } = [];

    public void Configure(EntityTypeBuilder<PersonaEntity> builder) {
        builder.HasKey(p => p.Id);
        builder.ComplexProperty(p => p.Characteristics);
        builder.HasMany(p => p.Tools)
               .WithMany()
               .UsingEntity<PersonaToolsEntity>(l => l.HasOne<ToolEntity>()
                                                      .WithMany()
                                                      .HasForeignKey(e => e.ToolId)
                                                      .OnDelete(DeleteBehavior.ClientCascade),
                                                r => r.HasOne<PersonaEntity>()
                                                      .WithMany()
                                                      .HasForeignKey(e => e.PersonaId)
                                                      .OnDelete(DeleteBehavior.ClientCascade));
    }
}
