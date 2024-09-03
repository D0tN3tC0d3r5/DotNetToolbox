namespace AI.Sample.Personas.Repositories;

public record Query {
    public required string Question { get; init; }
    public string Answer { get; set; } = string.Empty;
}
