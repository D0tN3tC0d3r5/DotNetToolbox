namespace AI.Sample.Personas.Jobs;

// Custom job classes for different AI tasks
public class GenerateQuestionJob(string id, JobContext context, ILogger<GenerateQuestionJob> logger)
    : Job<PersonaEntity, Query[]>(id, context) {
    public GenerateQuestionJob(IStringGuidProvider guid, JobContext context, ILogger<GenerateQuestionJob> logger)
        : this(guid.CreateSortable(), context, logger) {
    }
    public GenerateQuestionJob(JobContext context, ILogger<GenerateQuestionJob> logger)
        : this(StringGuidProvider.Default, context, logger) {
    }

    protected override Query[] MapResponse(TaskResponseType responseType, Message response) {
        try {
            var json = response.Text;
            var queries = JsonSerializer.Deserialize<Query[]>(response.Text);
            return queries ?? [];
        }
        catch (Exception ex) {
            logger.LogError(ex, "Failed to deserialize response: {JobResponse}", response.Text);
            throw;
        }
    }
}
