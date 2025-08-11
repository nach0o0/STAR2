using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Factories.ViewModel;
using WpfClient.Models;
using WpfClient.Security;
using WpfClient.Services.Application.Permission;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.Admin
{
    public partial class UserManagementViewModel : ViewModelBase
    {
        private readonly IViewModelFactory _factory;
        private readonly string _scope;

        // Das ViewModel für die linke Spalte (Benutzersuche und -liste)
        public UserListViewModel UserListViewModel { get; }

        // Das ViewModel für die rechte Spalte (wird dynamisch gesetzt)
        [ObservableProperty]
        private ViewModelBase? _detailViewModel;

        public UserManagementViewModel(
            IViewModelFactory factory,
            IPermissionService permissionService,
            string scope)
        {
            _factory = factory;
            _scope = scope;

            // Erstelle das ViewModel für die linke Spalte über die Factory
            UserListViewModel = _factory.CreateUserListViewModel(_scope);

            // Abonniere den PropertyChanged-Event, um auf eine Benutzerauswahl zu reagieren
            UserListViewModel.PropertyChanged += OnUserListPropertyChanged;
        }

        private void OnUserListPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UserListViewModel.SelectedUser) && UserListViewModel.SelectedUser != null)
            {
                // Wenn ein Benutzer ausgewählt wird, erstelle seine Detail-Ansicht.
                ShowUserDetails(UserListViewModel.SelectedUser);
            }
        }

        // Zeigt die Detail-Ansicht für einen Benutzer an und abonniert seine Events.
        private void ShowUserDetails(UserModel user)
        {
            var assignmentsVM = _factory.CreateUserAssignmentsViewModel(user, _scope);
            assignmentsVM.AssignRoleRequested += () => OnAssignRoleRequested(assignmentsVM);
            assignmentsVM.ManageDirectPermissionsRequested += () => OnManageDirectPermissionsRequested(assignmentsVM);
            DetailViewModel = assignmentsVM;
        }

        private void OnAssignRoleRequested(UserAssignmentsViewModel assignmentsVM)
        {
            var dialogVM = _factory.CreateAssignRoleDialogViewModel(assignmentsVM.User, _scope);

            dialogVM.RoleAssigned += (assignedRole) => {
                assignmentsVM.LoadAssignmentsCommand.Execute(null);
                DetailViewModel = assignmentsVM;
            };

            dialogVM.CloseRequested += () => {
                DetailViewModel = assignmentsVM;
            };

            DetailViewModel = dialogVM;
        }

        // NEUE METHODE: Öffnet den "Manage Direct Permissions"-Dialog.
        private void OnManageDirectPermissionsRequested(UserAssignmentsViewModel assignmentsVM)
        {
            var manageVM = _factory.CreateManageDirectPermissionsViewModel(
                assignmentsVM.User,
                _scope,
                assignmentsVM.DirectPermissions.ToList());

            manageVM.OnSave += () => {
                // Nach dem Speichern, lade die Details neu.
                ShowUserDetails(assignmentsVM.User);
            };
            manageVM.OnCancel += () => DetailViewModel = assignmentsVM;

            DetailViewModel = manageVM;
        }
    }
}
