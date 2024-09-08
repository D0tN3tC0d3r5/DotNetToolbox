namespace AI.Sample.Personas.Jobs;

public class GeneratePersonaJob(string id, JobContext context)
    : Job<PersonaEntity, string>(id, context) {
    public GeneratePersonaJob(IStringGuidProvider guid, JobContext context)
        : this(guid.CreateSortable(), context) {
    }
    public GeneratePersonaJob(JobContext context)
        : this(StringGuidProvider.Default, context) {
    }
}
