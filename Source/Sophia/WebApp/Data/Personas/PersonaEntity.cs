namespace Sophia.WebApp.Data.Personas;

[Table("Personas")]
[EntityTypeConfiguration(typeof(PersonaEntity))]
public class PersonaEntity
    : IEntityTypeConfiguration<PersonaEntity> {
    [Key]
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; } = "Agent";
    [MaxLength(1000)]
    public string Description { get; set; } = "You are a helpful agent.";
    [MaxLength(1000)]
    public string? Personality { get; set; }
    public string[] Instructions { get; set; } = [];
    public List<FactEntity> Facts { get; set; } = [];
    public List<ToolEntity> KnownTools { get; set; } = [];

    public void Configure(EntityTypeBuilder<PersonaEntity> builder) {
        builder.HasKey(p => p.Id);
        builder.PrimitiveCollection(p => p.Instructions);
        builder.OwnsMany(p => p.Facts)
               .ToTable("Persona_Facts");
        builder.HasMany(p => p.KnownTools)
               .WithMany()
               .UsingEntity("Persona_Tools",
                            r => r.HasOne(typeof(PersonaEntity)).WithMany().HasForeignKey("PersonaId").HasPrincipalKey(nameof(Id)),
                            l => l.HasOne(typeof(ToolEntity)).WithMany().HasForeignKey("ToolId").HasPrincipalKey(nameof(ToolEntity.Id)),
                            j => j.HasKey("PersonaId", "ToolId"));
    }

    public PersonaData ToDto()
        => new() {
                     Id = Id,
                     Name = Name,
                     Description = Description,
                     Personality = Personality,
                     Instructions = [..Instructions],
                     Facts = Facts.ToList(f => f.ToDto()),
                     KnownTools = KnownTools.ToList(f => f.ToDto()),
                 };
}
