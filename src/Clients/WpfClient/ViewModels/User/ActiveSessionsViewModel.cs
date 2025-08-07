using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Models;
using WpfClient.Services.Application.Auth;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels.User
{
    public partial class ActiveSessionsViewModel : ViewModelBase
    {
        private readonly IAuthService _authService;

        [ObservableProperty]
        private ObservableCollection<ActiveSessionModel> _activeSessions = new();

        public ActiveSessionsViewModel(IAuthService authService)
        {
            _authService = authService;

            LoadSessionsCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadSessions()
        {
            await ExecuteCommandAsync(async () =>
            {
                var sessions = await _authService.GetMyActiveSessionsAsync();
                ActiveSessions = new ObservableCollection<ActiveSessionModel>(sessions);
            });
        }

        [RelayCommand]
        private async Task Logout()
        {
            await _authService.LogoutAsync();
        }
    }
}
