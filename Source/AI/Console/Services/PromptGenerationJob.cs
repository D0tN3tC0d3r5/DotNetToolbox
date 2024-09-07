namespace AI.Sample.Services;

public class PromptGenerationJob(string id, JobContext context)
    : Job<PersonaEntity, string>(id, context) {
    public PromptGenerationJob(IStringGuidProvider guid, JobContext context)
        : this(guid.CreateSortable(), context) {
    }
    public PromptGenerationJob(JobContext context)
        : this(StringGuidProvider.Default, context) {
    }
}
