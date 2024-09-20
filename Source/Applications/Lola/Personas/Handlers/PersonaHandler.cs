using Task = System.Threading.Tasks.Task;
using ValidationException = DotNetToolbox.Results.ValidationException;

namespace Lola.Personas.Handlers;

public class PersonaHandler(IServiceProvider services, ILogger<PersonaHandler> logger)
    : IPersonaHandler {
    private readonly IModelHandler _modelHandler = services.GetRequiredService<IModelHandler>();
    private readonly IUserProfileHandler _userHandler = services.GetRequiredService<IUserProfileHandler>();
    private readonly IPersonaDataSource _dataSource = services.GetRequiredService<IPersonaDataSource>();
    private readonly ITaskHandler _taskHandler = services.GetRequiredService<ITaskHandler>();
    private readonly IAgentAccessor _connectionAccessor = services.GetRequiredService<IAgentAccessor>();

    public PersonaEntity[] List() => _dataSource.GetAll();

    public PersonaEntity? GetById(uint id) => _dataSource.FindByKey(id);
    public PersonaEntity? Find(Expression<Func<PersonaEntity, bool>> predicate)
        => _dataSource.Find(predicate);

    public void Add(PersonaEntity persona) {
        if (_dataSource.FindByKey(persona.Id) != null)
            throw new InvalidOperationException($"A persona with the id '{persona.Id}' already exists.");

        var context = Map.FromValue(nameof(PersonaHandler), this);
        var result = _dataSource.Add(persona, context);
        if (!result.IsSuccess)
            throw new ValidationException(result.Errors);
        logger.LogInformation("Added new persona: {PersonaId} => {PersonaName}", persona.Name, persona.Id);
    }

    public void Update(PersonaEntity persona) {
        if (_dataSource.FindByKey(persona.Id) == null)
            throw new InvalidOperationException($"Persona with id '{persona.Id}' not found.");

        var context = Map.FromValue(nameof(PersonaHandler), this);
        var result = _dataSource.Update(persona, context);
        if (!result.IsSuccess)
            throw new ValidationException(result.Errors);
        logger.LogInformation("Updated persona: {PersonaId} => {PersonaName}", persona.Name, persona.Id);
    }

    public void Remove(uint id) {
        var persona = _dataSource.FindByKey(id)
                     ?? throw new InvalidOperationException($"Persona with id '{id}' not found.");

        _dataSource.Remove(id);
        logger.LogInformation("Removed persona: {PersonaId} => {PersonaName}", persona.Name, persona.Id);
    }

    public async Task<Query[]> GenerateQuestion(PersonaEntity persona) {
        try {
            var appModel = _modelHandler.Selected ?? throw new InvalidOperationException("No default AI model selected.");
            var httpConnection = _connectionAccessor.GetFor(appModel.Provider!.Name);
            var userProfileEntity = _userHandler.CurrentUser ?? throw new InvalidOperationException("No user found.");
            var personaEntity = GetById(1) ?? throw new InvalidOperationException("Required persona not found. Name: 'Agent Creator'.");
            var taskEntity = _taskHandler.GetById(1) ?? throw new InvalidOperationException("Required task not found. Name: 'Ask Questions about the AI Agent'.");
            var context = new JobContext {
                Model = appModel,
                Agent = httpConnection,
                UserProfile = userProfileEntity,
                Persona = personaEntity,
                Task = taskEntity,
                Input = persona,
            };
            var job = new Job(context);
            job.Converters.Add(typeof(List<Query>),
                               v => {
                                   var list = (List<Query>)v;
                                   if (list.Count == 0) return string.Empty;
                                   var sb = new StringBuilder();
                                   foreach (var item in list) {
                                       sb.AppendLine($"Q: {item.Question}");
                                       sb.AppendLine($"{item.Explanation}");
                                       sb.AppendLine($"A: {item.Answer}");
                                   }
                                   return sb.ToString();
                               });
            var result = await job.Execute(CancellationToken.None);
            if (result.HasException) throw new("Failed to generate next question: " + result.Exception.Message);
            var response = context.OutputAsMap.GetList<Map>("Questions");
            return response.ToArray(i => new Query {
                Question = i.GetRequiredValueAs<string>(nameof(Query.Question)),
                Explanation = i.GetRequiredValueAs<string>(nameof(Query.Explanation)),
            });
        }
        catch (Exception ex) {
            logger.LogError(ex, "Error generating next question for persona {PersonaName}", persona.Name);
            throw;
        }
    }

    public async Task UpdateCreatedPersona(PersonaEntity persona) {
        try {
            var appModel = _modelHandler.Selected ?? throw new InvalidOperationException("No default AI model selected.");
            var httpConnection = _connectionAccessor.GetFor(appModel.Provider!.Name);
            var userProfileEntity = _userHandler.CurrentUser ?? throw new InvalidOperationException("No user found.");
            var personaEntity = GetById(1) ?? throw new InvalidOperationException("Required persona not found. Name: 'Agent Creator'.");
            var taskEntity = _taskHandler.GetById(2) ?? throw new InvalidOperationException("Required task not found. Name: 'Ask Questions about the AI Agent'.");
            var context = new JobContext {
                Model = appModel,
                Agent = httpConnection,
                UserProfile = userProfileEntity,
                Persona = personaEntity,
                Task = taskEntity,
                Input = persona,
            };
            var job = new Job(context);
            job.Converters.Add(typeof(List<Query>),
                               v => {
                                   var list = (List<Query>)v;
                                   if (list.Count == 0) return string.Empty;
                                   var sb = new StringBuilder();
                                   foreach (var item in list) {
                                       sb.AppendLine($"Q: {item.Question}");
                                       sb.AppendLine($"{item.Explanation}");
                                       sb.AppendLine($"A: {item.Answer}");
                                   }
                                   return sb.ToString();
                               });
            var result = await job.Execute(CancellationToken.None);
            if (result.HasException) throw new("Failed to generate next question: " + result.Exception.Message);
            persona.Role = context.OutputAsMap.GetRequiredValueAs<string>(nameof(Persona.Role));
            persona.Goals = context.OutputAsMap.GetRequiredList<string>(nameof(Persona.Goals));
            persona.Expertise = context.OutputAsMap.GetRequiredValueAs<string>(nameof(Persona.Expertise));
            persona.Characteristics = context.OutputAsMap.GetRequiredList<string>(nameof(Persona.Characteristics));
            persona.Requirements = context.OutputAsMap.GetRequiredList<string>(nameof(Persona.Requirements));
            persona.Restrictions = context.OutputAsMap.GetRequiredList<string>(nameof(Persona.Restrictions));
            persona.Characteristics = context.OutputAsMap.GetRequiredList<string>(nameof(Persona.Traits));
        }
        catch (Exception ex) {
            logger.LogError(ex, "Error generating next question for persona {PersonaName}", persona.Name);
            throw;
        }
    }
}
