namespace AI.Sample.Services;

// Custom job classes for different AI tasks
public class PersonaGenerationJob(string id, JobContext context)
    : Job<PersonaEntity, string>(id, context) {
    public PersonaGenerationJob(IStringGuidProvider guid, JobContext context)
        : this(guid.CreateSortable(), context) {
    }
    public PersonaGenerationJob(JobContext context)
        : this(StringGuidProvider.Default, context) {
    }
}
