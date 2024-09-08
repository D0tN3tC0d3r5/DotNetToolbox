namespace AI.Sample.Personas.Jobs;

// Custom job classes for different AI tasks
public class GenerateQuestionJob(string id, JobContext context)
    : Job<PersonaEntity, Query[]>(id, context) {
    public GenerateQuestionJob(IStringGuidProvider guid, JobContext context)
        : this(guid.CreateSortable(), context) {
    }
    public GenerateQuestionJob(JobContext context)
        : this(StringGuidProvider.Default, context) {
    }
}
