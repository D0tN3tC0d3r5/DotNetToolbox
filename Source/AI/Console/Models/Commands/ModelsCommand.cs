namespace AI.Sample.Models.Commands;

public class ModelsCommand : Command<ModelsCommand> {
    public ModelsCommand(IHasChildren parent) : base(parent, "Models", []) {
        Description = "Manage AI models.";

        AddCommand<ModelListCommand>();
        AddCommand<ModelAddCommand>();
        AddCommand<ModelUpdateCommand>();
        AddCommand<ModelRemoveCommand>();
        AddCommand<ModelInfoCommand>();
        AddCommand<HelpCommand>();
    }
}
