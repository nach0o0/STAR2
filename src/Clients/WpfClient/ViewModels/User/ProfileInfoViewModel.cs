using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using WpfClient.Models;
using WpfClient.Services.Application.MyEmployeeProfile;
using WpfClient.Services.Application.UserState;
using WpfClient.ViewModels.Base;
using CommunityToolkit.Mvvm.Messaging;
using WpfClient.Messages;
using WpfClient.Services.Application.Navigation;
using WpfClient.Services.Application.Notification;
using WpfClient.ViewModels.Shell;

namespace WpfClient.ViewModels.User
{
    public partial class ProfileInfoViewModel : ViewModelBase
    {
        private readonly IMyEmployeeProfileService _myProfileService;
        private readonly IUserStateService _userStateService;
        private readonly IMessenger _messenger;
        private readonly INavigationService _navigationService;

        [ObservableProperty]
        private MyEmployeeProfileModel _profile;

        public bool HasProfile => _userStateService.HasProfile;
        public string Email => _userStateService.CurrentUser?.Email ?? "N/A";

        public ProfileInfoViewModel(
            IMyEmployeeProfileService myProfileService,
            IUserStateService userStateService,
            IMessenger messenger,
            INavigationService navigationService)
        {
            _myProfileService = myProfileService;
            _userStateService = userStateService;
            _messenger = messenger;
            _navigationService = navigationService;

            _profile = _userStateService.Profile ?? new MyEmployeeProfileModel();
            _userStateService.PropertyChanged += UserStateService_PropertyChanged;
        }

        private void UserStateService_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IUserStateService.Profile) || e.PropertyName == nameof(IUserStateService.CurrentUser))
            {
                Profile = _userStateService.Profile ?? new MyEmployeeProfileModel();
                OnPropertyChanged(nameof(HasProfile));
                OnPropertyChanged(nameof(Email));
            }
        }

        [RelayCommand]
        private async Task CreateProfile()
        {
            await ExecuteCommandAsync(async () =>
            {
                await _myProfileService.CreateMyProfileAsync(Profile.FirstName, Profile.LastName);
            });
        }

        [RelayCommand(CanExecute = nameof(CanUpdateProfile))]
        private async Task UpdateProfile()
        {
            await ExecuteCommandAsync(async () =>
            {
                await _myProfileService.UpdateMyProfileAsync(Profile.FirstName, Profile.LastName);
                _messenger.Send(new StatusUpdateMessage("Profile updated successfully.", StatusMessageType.Success));
            });
        }

        private bool CanUpdateProfile() =>
            Profile != null &&
            !string.IsNullOrWhiteSpace(Profile.FirstName) &&
            !string.IsNullOrWhiteSpace(Profile.LastName);
    }
}
