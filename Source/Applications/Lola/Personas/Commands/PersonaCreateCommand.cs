using Task = System.Threading.Tasks.Task;
using ValidationException = DotNetToolbox.Results.ValidationException;

namespace Lola.Personas.Commands;

public class PersonaGenerateCommand(IHasChildren parent, IPersonaHandler personaHandler)
    : Command<PersonaGenerateCommand>(parent, "Generate", n => {
        n.Aliases = ["gen"];
        n.Description = "Generate a new persona";
        n.Help = "Generate a new agent persona using AI assistance.";
    }) {
    private const int _maxQuestions = 10;

    protected override Task<Result> ExecuteAsync(CancellationToken ct = default) => this.HandleCommandAsync(async lt => {
        try {
            Logger.LogInformation("Executing Personas->Generate command...");
            var persona = new PersonaEntity();
            await SetUpAsync(persona, lt);
            await AskAdditionalQuestions(persona, lt);
            await personaHandler.UpdateCreatedPersona(persona);

            Output.WriteLine($"[green]Agent persona '{persona.Name}' generated successfully.[/]");
            Logger.LogInformation("Persona '{PersonaKey}:{PersonaName}' generated successfully.", persona.Key, persona.Name);

            ShowResult(persona);

            var savePersona = await Input.ConfirmAsync("Are you ok with the generated Agent above?", lt);
            if (savePersona) {
                personaHandler.Add(persona);
                Logger.LogInformation("Persona '{PersonaKey}:{PersonaName}' added successfully.", persona.Key, persona.Name);
                return Result.Success();
            }

            Output.WriteLine("[yellow]Please review the provided answers and try again.[/]");
            return Result.Success();
        }
        catch (ValidationException ex) {
            var errors = string.Join("\n", ex.Errors.Select(e => $" - {e.Source}: {e.Message}"));
            Logger.LogWarning("Error generating the new persona. Validation errors:\n{Errors}", errors);
            Output.WriteLine($"[red]We found some problems while generating the new persona.\nPlease correct the following errors and try again.\n{errors}[/]");
            return Result.Invalid(ex.Errors);
        }
    }, "Error generating the new persona.", ct);

    private void ShowResult(PersonaEntity persona) {
        Output.WriteLine();
        Output.WriteLine($"[teal]Name:[/] {persona.Name}");
        Output.WriteLine($"[teal]Role:[/] {persona.Role}");
        Output.WriteLine("[teal]Goals:[/]");
        Output.WriteLine(string.Join("\n", persona.Goals.Select(i => $" - {i}")));
        Output.WriteLine("[teal]Expertise:[/] [green](auto-generated)[/]");
        Output.WriteLine(persona.Expertise);
        Output.WriteLine("[teal]Traits:[/] [green](auto-generated)[/]");
        Output.WriteLine(string.Join("\n", persona.Traits.Select(i => $" - {i}")));
        Output.WriteLine("[teal]Requirements:[/] [green](auto-generated)[/]");
        Output.WriteLine(string.Join("\n", persona.Important.Select(i => $" - {i}")));
        Output.WriteLine("[teal]Restrictions:[/] [green](auto-generated)[/]");
        Output.WriteLine(string.Join("\n", persona.Negative.Select(i => $" - {i}")));
        Output.WriteLine("[teal]Other:[/] [green](auto-generated)[/]");
        Output.WriteLine(string.Join("\n", persona.Other.Select(i => $" - {i}")));
        Output.WriteLine();
    }

    private async Task AskAdditionalQuestions(PersonaEntity persona, CancellationToken lt) {
        for (var questionCount = 0; questionCount < _maxQuestions; questionCount++) {
            Output.WriteLine("[yellow]Let me see if I have more questions...[/]");
            Output.WriteLine("[grey](You can skip the questions by typing 'proceed' at any time.)[/]");

            var queries = await personaHandler.GenerateQuestion(persona);
            if (queries.Length == 0) {
                Output.WriteLine("[green]I've gathered sufficient information to generate the agent's persona.[/]");
                break;
            }
            var proceed = false;
            foreach (var query in queries) {
                query.Answer = await Input.BuildMultilinePrompt($"Question {questionCount + 1}: {query.Question}")
                                          .ShowAsync(lt);
                if (query.Answer.Equals("proceed", StringComparison.OrdinalIgnoreCase)) {
                    proceed = true;
                    break;
                }
                persona.Questions.Add(query);
            }

            if (!proceed) continue;
            Output.WriteLine("[green]Ok. Let's proceed with the Agent's Persona generation.[/]");
            break;
        }
    }

    private async Task SetUpAsync(PersonaEntity persona, CancellationToken ct) {
        persona.Name = await Input.BuildTextPrompt<string>("How would you like to call the Agent?")
                                  .AddValidation(name => PersonaEntity.ValidateName(name, personaHandler))
                                  .ShowAsync(ct);
        persona.Role = await Input.BuildTextPrompt<string>($"What is the [white]{persona.Name}[/] primary role?")
                                  .AddValidation(PersonaEntity.ValidateRole)
                                  .ShowAsync(ct);

        var goal = await Input.BuildMultilinePrompt($"What is the Main Goal for the [white]{persona.Name}[/]?")
                              .Validate(PersonaEntity.ValidateGoal)
                              .ShowAsync(ct);
        persona.Goals.AddRange(goal.Replace("\r", "").Split("\n"));
        var addAnotherGoal = await Input.ConfirmAsync("Would you like to add another goal?", ct);
        while (addAnotherGoal) {
            goal = await Input.BuildMultilinePrompt("Additional goal: ")
                              .Validate(PersonaEntity.ValidateGoal)
                              .ShowAsync(ct);
            persona.Goals.AddRange(goal.Replace("\r", "").Split("\n"));
            addAnotherGoal = await Input.ConfirmAsync("Would you like to add another goal?", ct);
        }
    }
}
