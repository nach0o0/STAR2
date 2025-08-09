using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Factories.ViewModel;
using WpfClient.Models;
using WpfClient.Security;
using WpfClient.Services.Application.Permission;
using WpfClient.Services.Application.PermissionAdmin;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.Admin
{
    public partial class RoleManagementViewModel : ViewModelBase
    {
        private readonly IViewModelFactory _factory;
        private readonly string _scope;

        // Das ViewModel für die linke Spalte (die Liste der Rollen)
        public RoleListViewModel RoleListViewModel { get; }

        // Das ViewModel für die rechte Spalte (wird dynamisch gesetzt)
        [ObservableProperty]
        private ViewModelBase? _detailViewModel;

        public RoleManagementViewModel(
            IViewModelFactory factory,
            string scope)
        {
            _factory = factory;
            _scope = scope;

            // Erstelle das ViewModel für die linke Spalte
            RoleListViewModel = _factory.CreateRoleListViewModel(_scope);

            // Abonniere die Events des Kind-ViewModels, um auf Aktionen zu reagieren
            RoleListViewModel.PropertyChanged += OnRoleListPropertyChanged;
        }

        private async void OnRoleListPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            // Wenn in der Liste eine Rolle ausgewählt wird...
            if (e.PropertyName == nameof(RoleListViewModel.SelectedRole))
            {
                if (RoleListViewModel.SelectedRole != null)
                {
                    var detailsVM = _factory.CreateRoleDetailsViewModel(RoleListViewModel.SelectedRole, _scope);
                    detailsVM.RoleDeleted += OnRoleDeleted;
                    DetailViewModel = detailsVM;
                }
                else
                {
                    // Wenn die Auswahl aufgehoben wird, leere die Detailansicht.
                    DetailViewModel = null;
                }
            }
            // Wenn der "Create"-Button geklickt wird...
            else if (e.PropertyName == nameof(RoleListViewModel.CreateNewRoleRequested) && RoleListViewModel.CreateNewRoleRequested)
            {
                RoleListViewModel.SelectedRole = null; // Auswahl aufheben
                var createVM = _factory.CreateCreateRoleViewModel(_scope);

                createVM.PropertyChanged += OnNewRoleCreated;
                createVM.CancelRequested += OnCreateCancelled;

                DetailViewModel = createVM;
                RoleListViewModel.CreateNewRoleRequested = false;
            }
        }

        // Event-Handler für die Erstellung einer neuen Rolle
        private void OnNewRoleCreated(object? sender, PropertyChangedEventArgs e)
        {
            var createVM = sender as CreateRoleViewModel;
            if (e.PropertyName == nameof(CreateRoleViewModel.NewlyCreatedRole) && createVM?.NewlyCreatedRole != null)
            {
                RoleListViewModel.Roles.Add(createVM.NewlyCreatedRole);
                RoleListViewModel.SelectedRole = createVM.NewlyCreatedRole;
            }
        }

        // Event-Handler für das Löschen einer Rolle
        private void OnRoleDeleted(Guid roleId)
        {
            var roleToRemove = RoleListViewModel.Roles.FirstOrDefault(r => r.Id == roleId);
            if (roleToRemove != null)
            {
                RoleListViewModel.Roles.Remove(roleToRemove);
            }
            DetailViewModel = null; // Detailansicht leeren
        }

        // Event-Handler für den Abbruch
        private void OnCreateCancelled()
        {
            DetailViewModel = null; // Detailansicht leeren
        }
    }
}
