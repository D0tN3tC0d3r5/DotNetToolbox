namespace AI.Sample.Services;

// Custom job classes for different AI tasks
public class PersonaGenerationJob(JobContext context, IServiceProvider services)
    : Job<PersonaEntity, string>(new PersonaGenerationJobStrategy(services), context);
