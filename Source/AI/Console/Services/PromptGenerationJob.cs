namespace AI.Sample.Services;

public class PromptGenerationJob(JobContext context, IServiceProvider services)
    : Job<PersonaEntity, string>(new PromptGenerationJobStrategy(services), context);
