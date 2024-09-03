namespace AI.Sample.Services;

// Custom job classes for different AI tasks
public class PersonaGenerationJob(JobContext context)
    : Job<PersonaEntity, string>(new PersonaGenerationJobStrategy(), Guid.NewGuid().ToString(), context);
