namespace Sophia.WebApp.Data.Personas;

[Table("Personas")]
[EntityTypeConfiguration(typeof(PersonaEntity))]
public class PersonaEntity
    : IEntityTypeConfiguration<PersonaEntity>
    , IHasTools<int>
    , IHasFacts<int> {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; } = "Agent";
    [MaxLength(1000)]
    public string Description { get; set; } = "You are a helpful ASSISTANT.";
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
                                                      .HasForeignKey(e => e.ToolId),
                                                r => r.HasOne<PersonaEntity>()
                                                      .WithMany()
                                                      .HasForeignKey(e => e.PersonaId));
    }

    public static async Task Seed(ApplicationDbContext dbContext) {
        if (await dbContext.Personas.AnyAsync()) return;
        var persona = new PersonaEntity();
        dbContext.Personas.Add(persona);
    }

    public PersonaData ToDto()
        => new() {
            Id = Id,
            Name = Name,
            Description = Description,
            Characteristics = Characteristics.ToDto(),
            Facts = Facts,
            KnownTools = Tools.ToList(f => f.ToDto()),
        };
}
