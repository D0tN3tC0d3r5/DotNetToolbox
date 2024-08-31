
using AI.Sample.Main.Commands;

namespace AI.Sample.Providers.Commands;

public class ProvidersCommand
    : Command<ProvidersCommand> {
    public ProvidersCommand(IHasChildren parent)
        : base(parent, "Providers", []) {
        Description = "Manage AI providers.";

        AddCommand<ProviderListCommand>();
        AddCommand<ProviderAddCommand>();
        AddCommand<ProviderUpdateCommand>();
        AddCommand<ProviderRemoveCommand>();
        AddCommand<HelpCommand>();
    }
}
