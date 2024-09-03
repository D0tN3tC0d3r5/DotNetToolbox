namespace AI.Sample.Services;

public class PromptGenerationJob(JobContext context)
    : Job<PersonaEntity, string>(new PromptGenerationJobStrategy(), Guid.NewGuid().ToString(), context);
