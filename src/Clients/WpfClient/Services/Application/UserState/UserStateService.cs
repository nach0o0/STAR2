using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using Refit;
using System.Net;
using WpfClient.Messages;
using WpfClient.Models;
using WpfClient.Services.Api.Interfaces;
using WpfClient.Services.Application.Auth;

namespace WpfClient.Services.Application.UserState
{
    public partial class UserStateService : ObservableObject, IUserStateService, 
        IRecipient<UserLoggedInMessage>, IRecipient<UserLoggedOutMessage>
    {
        private readonly IOrganizationApiClient _organizationApiClient;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsLoggedIn))]
        private CurrentUser? _currentUser;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasProfile))]
        private MyEmployeeProfileModel? _profile;

        public bool IsLoggedIn => CurrentUser != null;
        public bool HasProfile => Profile != null;

        public UserStateService(IOrganizationApiClient organizationApiClient, IMessenger messenger)
        {
            _organizationApiClient = organizationApiClient;
            messenger.RegisterAll(this);
        }

        public async void Receive(UserLoggedInMessage message)
        {
            CurrentUser = message.User;
            await RefreshProfileAsync();
        }

        public void Receive(UserLoggedOutMessage message)
        {
            CurrentUser = null;
            Profile = null;
        }

        public async Task RefreshProfileAsync()
        {
            if (!IsLoggedIn)
            {
                Profile = null;
                return;
            }

            try
            {
                var profileDto = await _organizationApiClient.GetMyEmployeeProfileAsync();
                if (profileDto != null)
                {
                    Profile = new MyEmployeeProfileModel
                    {
                        EmployeeId = profileDto.EmployeeId,
                        FirstName = profileDto.FirstName,
                        LastName = profileDto.LastName
                    };
                }
                else
                {
                    Profile = null;
                }
            }
            catch (ApiException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                Profile = null;
            }
        }
    }
}
