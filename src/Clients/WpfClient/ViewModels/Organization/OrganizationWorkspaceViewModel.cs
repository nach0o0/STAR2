using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WpfClient.Factories.ViewModel;
using WpfClient.Models;
using WpfClient.Security;
using WpfClient.Services.Application.Auth;
using WpfClient.Services.Application.Organization;
using WpfClient.Services.Application.Permission;
using WpfClient.Services.Application.PermissionAdmin;
using WpfClient.Services.Application.UserAdmin;
using WpfClient.ViewModels.Admin;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.Organization
{
    public partial class OrganizationWorkspaceViewModel : ViewModelBase
    {
        private readonly IViewModelFactory _factory;
        private readonly IPermissionService _permissionService;
        private readonly IOrganizationService _organizationService;
        private readonly IAuthService _authService;

        // Das ViewModel für die linke Spalte (Liste der Organisationen)
        public OrganizationListViewModel OrganizationListViewModel { get; }

        // Der dynamische Platzhalter für den Inhalt der rechten Spalte
        [ObservableProperty]
        private ViewModelBase? _detailViewModel;

        // Steuert die Sichtbarkeit des "Neue Organisation"-Buttons
        public bool CanCreateTopLevelOrganization { get; }

        public OrganizationWorkspaceViewModel(
            IViewModelFactory factory,
            IPermissionService permissionService,
            IOrganizationService organizationService,
            IAuthService authService)
        {
            _factory = factory;
            _permissionService = permissionService;
            _organizationService = organizationService;
            _authService = authService;

            // Erstelle das ViewModel für die linke Spalte über die Factory
            OrganizationListViewModel = _factory.CreateOrganizationListViewModel();
            // Abonniere die Events des Listen-ViewModels, um auf Aktionen zu reagieren
            OrganizationListViewModel.PropertyChanged += OnOrganizationListPropertyChanged;

            // Prüfe die Berechtigung einmalig beim Laden
            CanCreateTopLevelOrganization = _permissionService.HasPermissionInScope(Security.PermissionKeys.OrganizationCreate, "global");
        }

        private void OnOrganizationListPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            // Reaktion auf die Auswahl einer Organisation in der Liste
            if (e.PropertyName == nameof(OrganizationListViewModel.SelectedOrganization))
            {
                if (OrganizationListViewModel.SelectedOrganization != null)
                {
                    // Erstelle und zeige das Detail-ViewModel für die ausgewählte Organisation an.
                    DetailViewModel = _factory.CreateOrganizationDetailsViewModel(OrganizationListViewModel.SelectedOrganization);
                }
                else
                {
                    DetailViewModel = null;
                }
            }
            // Reaktion auf den Klick des "Create New"-Buttons
            else if (e.PropertyName == nameof(OrganizationListViewModel.CreateNewRequested) && OrganizationListViewModel.CreateNewRequested)
            {
                ShowCreateOrganizationForm();
                OrganizationListViewModel.CreateNewRequested = false; // Signal zurücksetzen
            }
        }

        private void ShowCreateOrganizationForm()
        {
            OrganizationListViewModel.SelectedOrganization = null;

            var createVM = _factory.CreateCreateOrganizationViewModel(null);

            // Lausche auf den PropertyChanged-Event
            createVM.PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName == nameof(CreateOrganizationViewModel.NewlyCreatedOrganization) && createVM.NewlyCreatedOrganization != null)
                {
                    // 1. Hole ein neues, aktualisiertes Token mit den neuen Berechtigungen.
                    //    Dies aktualisiert den CurrentUser im UserStateService.
                    await _authService.TryInitializeSessionAsync();

                    // 2. Lade die Organisationsliste neu. Sie wird jetzt die neuen
                    //    Berechtigungen aus dem aktualisierten Token verwenden.
                    await OrganizationListViewModel.LoadOrganizationsCommand.ExecuteAsync(null);

                    // 3. Wähle die neu erstellte Organisation in der aktualisierten Liste aus.
                    OrganizationListViewModel.SelectedOrganization = OrganizationListViewModel.Organizations
                        .FirstOrDefault(o => o.Id == createVM.NewlyCreatedOrganization.Id);
                }
            };

            createVM.CancelRequested += () => DetailViewModel = null;

            DetailViewModel = createVM;
        }

        //private void ShowCreateOrganizationForm()
        //{
        //    OrganizationListViewModel.SelectedOrganization = null; // Auswahl in der Liste aufheben

        //    var createVM = _factory.CreateCreateOrganizationViewModel(null); // null = Top-Level

        //    // Lausche darauf, ob eine neue Organisation erstellt wurde.
        //    createVM.PropertyChanged += (s, e) =>
        //    {
        //        if (e.PropertyName == nameof(CreateOrganizationViewModel.NewlyCreatedOrganization) && createVM.NewlyCreatedOrganization != null)
        //        {
        //            // Füge die neue Organisation zur Liste hinzu und wähle sie aus.
        //            OrganizationListViewModel.Organizations.Add(createVM.NewlyCreatedOrganization);
        //            OrganizationListViewModel.SelectedOrganization = createVM.NewlyCreatedOrganization;
        //        }
        //    };

        //    // Lausche auf den "Cancel"-Button im Create-Formular.
        //    createVM.CancelRequested += () => DetailViewModel = null;

        //    DetailViewModel = createVM;
        //}
    }
    //public partial class OrganizationWorkspaceViewModel : ViewModelBase
    //{
    //    private readonly IViewModelFactory _factory;
    //    private readonly IPermissionService _permissionService;
    //    private readonly IOrganizationService _organizationService;

    //    // --- Listen- und Auswahl-Eigenschaften ---
    //    [ObservableProperty]
    //    private ObservableCollection<OrganizationModel> _organizations = new();

    //    [ObservableProperty]
    //    [NotifyPropertyChangedFor(nameof(IsOrganizationSelected))]
    //    private OrganizationModel? _selectedOrganization;

    //    public bool IsOrganizationSelected => SelectedOrganization != null;

    //    [ObservableProperty]
    //    private ViewModelBase? _detailViewModel;

    //    // --- Kind-ViewModels für die Tabs ---
    //    public CreateOrganizationViewModel CreateOrganizationViewModel { get; }

    //    [ObservableProperty]
    //    private RoleManagementViewModel? _roleManagementViewModel;

    //    [ObservableProperty]
    //    private UserManagementViewModel? _userManagementViewModel;

    //    // --- Eigenschaften zur Steuerung der UI ---
    //    public bool CanCreateTopLevelOrganization { get; }
    //    [ObservableProperty] private bool _canManageRoles;
    //    [ObservableProperty] private bool _canManageUsers;

    //    public OrganizationWorkspaceViewModel(
    //        IViewModelFactory factory,
    //        IPermissionService permissionService,
    //        IOrganizationService organizationService)
    //    {
    //        _factory = factory;
    //        _permissionService = permissionService;
    //        _organizationService = organizationService;

    //        CanCreateTopLevelOrganization = _permissionService.HasPermissionInScope(PermissionKeys.OrganizationCreate, "global");

    //        // Lade die Organisationen beim Start des Workspaces
    //        LoadOrganizationsCommand.Execute(null);
    //    }

    //    // Beobachtet Änderungen an der ausgewählten Organisation
    //    partial void OnSelectedOrganizationChanged(OrganizationModel? value)
    //    {
    //        DetailViewModel = null;
    //        CanManageRoles = false;
    //        CanManageUsers = false;

    //        if (value is null) return;

    //        // Erstelle die Kind-ViewModels mit dem korrekten, neuen Scope
    //        var scope = $"organization:{value.Id}";

    //        // Prüfe die Berechtigungen für die Tabs
    //        CanManageRoles = _permissionService.HasAnyPermissionInScope(new[] { PermissionKeys.RoleRead, PermissionKeys.RoleCreate }, scope);
    //        CanManageUsers = _permissionService.HasAnyPermissionInScope(new[] { PermissionKeys.AssignmentRead, PermissionKeys.PermissionAssignRole }, scope);

    //        // Erstelle die ViewModels für die Tabs, wenn die Berechtigung vorhanden ist
    //        if (CanManageRoles) RoleManagementViewModel = _factory.CreateRoleManagementViewModel(scope);
    //        if (CanManageUsers) UserManagementViewModel = _factory.CreateUserManagementViewModel(scope);

    //        if (DetailViewModel is CreateOrganizationViewModel)
    //        {
    //            DetailViewModel = null;
    //        }
    //    }

    //    [RelayCommand]
    //    private async Task LoadOrganizationsAsync()
    //    {
    //        await ExecuteCommandAsync(async () =>
    //        {
    //            var orgs = await _organizationService.GetRelevantOrganizationsAsync();
    //            Organizations = new ObservableCollection<OrganizationModel>(orgs);
    //        });
    //    }

    //    [RelayCommand(CanExecute = nameof(CanCreateTopLevelOrganization))]
    //    private void ShowCreateOrganizationForm()
    //    {
    //        SelectedOrganization = null; // Auswahl aufheben

    //        var createVM = _factory.CreateCreateOrganizationViewModel("global"); // `global` für Top-Level

    //        // Lausche auf Erfolg
    //        createVM.PropertyChanged += (s, e) => {
    //            if (e.PropertyName == nameof(CreateOrganizationViewModel.NewlyCreatedOrganization) && createVM.NewlyCreatedOrganization != null)
    //            {
    //                Organizations.Add(createVM.NewlyCreatedOrganization);
    //                SelectedOrganization = createVM.NewlyCreatedOrganization;
    //            }
    //        };
    //        // Lausche auf Abbruch
    //        createVM.CancelRequested += () => DetailViewModel = null;

    //        DetailViewModel = createVM;
    //    }
    //}
}
