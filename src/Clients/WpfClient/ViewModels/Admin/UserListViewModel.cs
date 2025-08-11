using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Models;
using WpfClient.Services.Application.UserAdmin;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.Admin
{
    public partial class UserListViewModel : ViewModelBase
    {
        private readonly IUserAdminService _userAdminService;
        private readonly string _scope;

        [ObservableProperty]
        private ObservableCollection<UserModel> _relevantUsers = new();

        [ObservableProperty]
        private UserModel? _selectedUser;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(FindUserByEmailCommand))]
        private string _emailToFind = string.Empty;

        public UserListViewModel(IUserAdminService userAdminService, string scope)
        {
            _userAdminService = userAdminService;
            _scope = scope;

            LoadRelevantUsersCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadRelevantUsersAsync()
        {
            await ExecuteCommandAsync(async () =>
            {
                var users = await _userAdminService.GetRelevantUsersForScopeAsync(_scope);
                RelevantUsers = new ObservableCollection<UserModel>(users);
                SelectedUser = null;
            });
        }

        [RelayCommand(CanExecute = nameof(CanFindUser))]
        private async Task FindUserByEmailAsync()
        {
            await ExecuteCommandAsync(async () =>
            {
                var foundUser = await _userAdminService.FindUserByEmailAsync(EmailToFind, _scope);
                if (foundUser != null)
                {
                    // Prüfe, ob der Benutzer bereits in der Liste ist
                    if (!RelevantUsers.Any(u => u.Id == foundUser.Id))
                    {
                        RelevantUsers.Add(foundUser);
                    }
                    // Wähle den gefundenen Benutzer aus
                    SelectedUser = RelevantUsers.FirstOrDefault(u => u.Id == foundUser.Id);
                    EmailToFind = string.Empty;
                }
            });
        }
        private bool CanFindUser() => !string.IsNullOrWhiteSpace(EmailToFind);
    }
}
