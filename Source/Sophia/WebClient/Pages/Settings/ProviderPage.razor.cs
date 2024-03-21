namespace Sophia.WebClient.Pages.Settings;

public partial class ProviderPage {
    [Parameter] public int? Id { get; set; }

    private ProviderData _provider = new();
    private EditContext _editContext;
    private ValidationMessageStore _validationMessageStore;

    private PageAction _action;
    [Parameter]
    [SuppressMessage("Usage", "BL0007:Component parameters should be auto properties", Justification = "<Pending>")]
    public string Action {
        get => _action.ToString();
        set => _action = Enum.Parse<PageAction>(value);
    }
    public bool IsReadOnly => _action == PageAction.View;

    [Inject] public IProvidersService ProvidersService { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;

    public ProviderPage() {
        _editContext = new(_provider);
        _validationMessageStore = new(_editContext);
    }

    protected override async Task OnInitializedAsync() {
        _provider = await GetProviderById(Id);
        _action = Enum.Parse<PageAction>(Action);
    }

    protected override void OnParametersSet() {
        _editContext = new(_provider);
        _validationMessageStore = new(_editContext);
        _editContext.OnValidationRequested += OnValidationRequested;
        base.OnParametersSet();
    }

    private void OnValidationRequested(object? sender, ValidationRequestedEventArgs e) {
        _validationMessageStore.Clear(() => _provider.Models);
        var result = _provider.ValidateModels();
        if (result != null) _validationMessageStore.Add(() => _provider.Models, result);
    }

    public async Task<ProviderData> GetProviderById(int? providerId)
        => providerId is null
               ? new()
               : await ProvidersService.GetById(providerId.Value);

    public void EnableEdit() => _action = PageAction.Edit;

    public async Task Save() {
        if (_provider.Id == 0) await ProvidersService.Add(_provider);
        else await ProvidersService.Update(_provider);
        _action = PageAction.View;
    }

    public async Task Cancel() {
        if (_action == PageAction.Edit) await CancelEdit();
        else GoBack();
        _action = PageAction.View;
    }

    public async Task CancelEdit() {
        _action = PageAction.View;
        _provider = await GetProviderById(Id);
    }

    public void GoBack() => NavigationManager.NavigateTo("/settings/providers");

    private void InsertOption(int index = -1) {
        var model = new ModelData();
        if (index == _provider.Models.Count - 1) _provider.Models.Add(model);
        else _provider.Models.Insert(index + 1, model);
    }

    private void RemoveOption(int index)
        => _provider.Models.RemoveAt(index);
}
