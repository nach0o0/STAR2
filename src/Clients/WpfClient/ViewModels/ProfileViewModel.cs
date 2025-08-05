using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Session.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfClient.Exceptions;
using WpfClient.Messages;
using WpfClient.Models;
using WpfClient.Services.Api.AuthApi;
using WpfClient.Services.Api.OrganizationApi;
using WpfClient.Services.Application.Auth;
using WpfClient.Services.Application.MyEmployeeProfile;
using WpfClient.Services.Application.Notification;
using WpfClient.ViewModels.Base;

namespace WpfClient.ViewModels
{
    public partial class ProfileViewModel : ViewModelBase
    {
        private readonly IAuthService _authService;
        private readonly IMyEmployeeProfileService _myProfileService;
        private readonly INotificationService _notificationService;

        [ObservableProperty]
        private string _email = "Loading...";

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangePasswordCommand))]
        private string _oldPassword = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangePasswordCommand))]
        private string _newPassword = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(ChangePasswordCommand))]
        private string _confirmNewPassword = string.Empty;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteAccountCommand))]
        private string _confirmPassword = string.Empty;

        [ObservableProperty]
        private MyEmployeeProfileModel? _profile;

        [ObservableProperty]
        private bool _hasProfile;

        [ObservableProperty]
        private ObservableCollection<ActiveSessionModel> _activeSessions = new();

        [ObservableProperty]
        private string? _errorMessage;

        [ObservableProperty]
        private string? _successMessage;

        public ProfileViewModel(IAuthService authService, IMyEmployeeProfileService myProfileService, INotificationService notificationService)
        {
            _authService = authService;
            _myProfileService = myProfileService;
            _notificationService = notificationService;

            _myProfileService.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(IMyEmployeeProfileService.CurrentProfile))
                {
                    Profile = _myProfileService.CurrentProfile ?? new MyEmployeeProfileModel();
                    HasProfile = _myProfileService.CurrentProfile is not null;
                }
            };

            Profile = _myProfileService.CurrentProfile ?? new MyEmployeeProfileModel();
            HasProfile = _myProfileService.CurrentProfile is not null;
            if (_authService.CurrentUser is not null) Email = _authService.CurrentUser.Email;
            LoadSessionsCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadSessions()
        {
            try
            {
                var sessions = await _authService.GetMyActiveSessionsAsync();
                ActiveSessions = new ObservableCollection<ActiveSessionModel>(sessions);
            }
            catch (ApiException) { /* Fehler behandeln */ }
        }

        [RelayCommand]
        private async Task CreateProfile()
        {
            if (Profile is null) return;
            ClearMessages();
            try
            {
                await _myProfileService.CreateMyProfileAsync(Profile.FirstName, Profile.LastName);
                SuccessMessage = "Profile created successfully.";
            }
            catch (ApiException) { ErrorMessage = "Failed to create profile."; }
        }

        [RelayCommand(CanExecute = nameof(CanUpdateProfile))]
        private async Task UpdateProfile()
        {
            if (Profile is null) return;
            ClearMessages();
            try
            {
                await _myProfileService.UpdateMyProfileAsync(Profile.FirstName, Profile.LastName);
                SuccessMessage = "Profile updated successfully.";
            }
            catch (ApiException) { ErrorMessage = "Failed to update profile."; }
        }


        [RelayCommand]
        private async Task Logout()
        {
            await _authService.LogoutAsync();
        }

        [RelayCommand(CanExecute = nameof(CanChangePassword))]
        private async Task ChangePassword()
        {
            ClearMessages();

            if (NewPassword != ConfirmNewPassword)
            {
                ErrorMessage = "New passwords do not match.";
                return;
            }

            try
            {
                await _authService.ChangePasswordAsync(OldPassword, NewPassword);
                _notificationService.SetMessage("Password changed successfully. Please log in again.");
                await _authService.LogoutAsync();
            }
            catch (ApiException ex)
            {
                ErrorMessage = "Failed to change password. Please check your old password.";
            }
        }

        [RelayCommand(CanExecute = nameof(CanDeleteAccount))]
        private async Task DeleteAccount()
        {
            ClearMessages();
            try
            {
                _notificationService.SetMessage("Account deleted successfully.");
                await _authService.DeleteMyAccountAsync(ConfirmPassword);
            }
            catch (ApiException ex)
            {
                ErrorMessage = "Failed to delete account. Please check your password.";
            }
        }

        private void ClearMessages()
        {
            ErrorMessage = null;
            SuccessMessage = null;
        }

        private bool CanUpdateProfile()
        {
            return Profile is not null &&
                   !string.IsNullOrWhiteSpace(Profile.FirstName) &&
                   !string.IsNullOrWhiteSpace(Profile.LastName);
        }

        private bool CanChangePassword() =>
            !string.IsNullOrEmpty(OldPassword) &&
            !string.IsNullOrEmpty(NewPassword) &&
            !string.IsNullOrEmpty(ConfirmNewPassword);

        private bool CanDeleteAccount() => !string.IsNullOrEmpty(ConfirmPassword);
    }
}
