namespace Sophia.WebApp.Data.Personas;

[Table("PersonaFacts")]
public class PersonaFactsEntity {
    public int PersonaId { get; set; }
    public int FactId { get; set; }
}