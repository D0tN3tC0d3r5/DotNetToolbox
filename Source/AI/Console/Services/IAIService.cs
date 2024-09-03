namespace AI.Sample.Services;

public interface IAIService {
    Task<string> GetNextQuestion(PersonaEntity persona);
    //Task<string> GeneratePrompt(PersonaEntity persona);
}
